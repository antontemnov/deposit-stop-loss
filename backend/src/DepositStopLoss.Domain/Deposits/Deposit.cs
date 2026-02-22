using System;
using System.Collections.Generic;
using System.Linq;

using DepositStopLoss.Domain.Banking;
using DepositStopLoss.Domain.Events;
using DepositStopLoss.Domain.SharedKernel;
using DepositStopLoss.Domain.Users;

using NodaTime;

namespace DepositStopLoss.Domain.Deposits;

/// <summary>
///     Deposit aggregate root. Represents user's deposit in foreign currency.
///     Core business entity with profitability tracking.
/// </summary>
public sealed class Deposit : AggregateRoot<DepositIdentity>
{
    private readonly List<DepositContribution> _contributions = new();

    private Deposit()
    {
    }

    private Deposit(
        DepositIdentity id,
        UserIdentity userId,
        BankIdentity bankId,
        Currency currency,
        Percentage annualInterestRate,
        Instant openedAt,
        int termMonths,
        RateType rateType,
        Percentage stopLossThreshold,
        DepositSource source)
        : base(id)
    {
        UserId = userId;
        BankId = bankId;
        Currency = currency;
        AnnualInterestRate = annualInterestRate;
        OpenedAt = openedAt;
        TermMonths = termMonths;
        MaturityDate = CalculateMaturityDate(openedAt, termMonths);
        RateType = rateType;
        StopLossThreshold = stopLossThreshold;
        Source = source;
        Status = DepositStatus.Active;
    }

    /// <summary>
    ///     Annual interest rate percentage.
    /// </summary>
    public Percentage AnnualInterestRate { get; private set; } = null!;

    /// <summary>
    ///     Bank where deposit is held.
    /// </summary>
    public BankIdentity BankId { get; private set; }

    /// <summary>
    ///     When deposit was closed (null if still active).
    /// </summary>
    public Instant? ClosedAt { get; private set; }

    /// <summary>
    ///     Contributions (пополнения).
    /// </summary>
    public IReadOnlyList<DepositContribution> Contributions => _contributions.AsReadOnly();

    /// <summary>
    ///     Deposit currency. All contributions must be in this currency.
    /// </summary>
    public Currency Currency { get; private set; }

    /// <summary>
    ///     Current total amount (sum of all contributions). Derived from contributions.
    /// </summary>
    public decimal CurrentAmount => _contributions.Sum(c => c.Amount);

    /// <summary>
    ///     Maturity date (calculated from OpenedAt + TermMonths).
    /// </summary>
    public LocalDate MaturityDate { get; private set; }

    /// <summary>
    ///     When deposit was opened.
    /// </summary>
    public Instant OpenedAt { get; private set; }

    /// <summary>
    ///     Exchange rate type (Commercial or Discounted).
    /// </summary>
    public RateType RateType { get; private set; }

    /// <summary>
    ///     Source of deposit data (Manual, TBC API, BOG API).
    /// </summary>
    public DepositSource Source { get; }

    /// <summary>
    ///     Deposit status (Active, Paused, Closed).
    /// </summary>
    public DepositStatus Status { get; private set; }

    /// <summary>
    ///     Stop-loss threshold percentage (e.g., -2% = sell when profit drops below -2%).
    /// </summary>
    public Percentage StopLossThreshold { get; private set; } = null!;

    /// <summary>
    ///     Deposit term in months.
    /// </summary>
    public int TermMonths { get; private set; }

    /// <summary>
    ///     Owner of the deposit.
    /// </summary>
    public UserIdentity UserId { get; private set; }

    /// <summary>
    ///     Factory method to create new deposit (without initial contribution).
    /// </summary>
    public static Deposit Create(
        UserIdentity userId,
        BankIdentity bankId,
        Currency currency,
        Percentage annualInterestRate,
        Instant openedAt,
        int termMonths,
        RateType rateType,
        Percentage stopLossThreshold,
        DepositSource source,
        Instant now)
    {
        if (termMonths <= 0)
        {
            throw new ArgumentException("Term must be positive", nameof(termMonths));
        }

        var deposit = new Deposit(
            DepositIdentity.New(),
            userId,
            bankId,
            currency,
            annualInterestRate,
            openedAt,
            termMonths,
            rateType,
            stopLossThreshold,
            source);

        deposit.AddDomainEvent(new DepositCreatedEvent(deposit.Id, userId, now));

        return deposit;
    }

    /// <summary>
    ///     Add funds to deposit (пополнение).
    /// </summary>
    public void AddFunds(decimal amount, decimal exchangeRate, Instant contributedAt, Instant now)
    {
        if (Status is not DepositStatus.Active)
        {
            throw new InvalidOperationException("Can only add funds to active deposit");
        }

        if (exchangeRate <= 0)
        {
            throw new ArgumentException("Exchange rate must be positive", nameof(exchangeRate));
        }

        var contribution = DepositContribution.Create(Id, amount, contributedAt, exchangeRate);
        _contributions.Add(contribution);

        AddDomainEvent(new DepositFundsAddedEvent(Id, amount, exchangeRate, now));
    }

    /// <summary>
    ///     Calculate weighted average exchange rate across all contributions.
    ///     Used for profitability calculation when deposit has multiple contributions.
    /// </summary>
    public decimal CalculateWeightedAverageExchangeRate(Instant asOf)
    {
        if (_contributions.Count is 0)
        {
            throw new InvalidOperationException("No contributions found");
        }

        if (_contributions.Count is 1)
        {
            return _contributions[0].ExchangeRateAtContribution;
        }

        decimal totalWeighted = _contributions.Sum(c => c.Amount * c.ExchangeRateAtContribution * c.DaysFromContribution(asOf));

        decimal totalWeights = _contributions.Sum(c => c.Amount * c.DaysFromContribution(asOf));

        return totalWeighted / totalWeights;
    }

    /// <summary>
    ///     Check if deposit details can be edited.
    ///     API-imported deposits are read-only.
    /// </summary>
    public bool CanEditDetails()
    {
        return Source is DepositSource.Manual;
    }

    /// <summary>
    ///     Close deposit.
    /// </summary>
    public void Close(Instant closedAt)
    {
        if (Status is DepositStatus.Closed)
        {
            throw new InvalidOperationException("Deposit already closed");
        }

        Status = DepositStatus.Closed;
        ClosedAt = closedAt;

        AddDomainEvent(new DepositClosedEvent(Id, closedAt, closedAt));
    }

    /// <summary>
    ///     Pause deposit monitoring.
    /// </summary>
    public void Pause(Instant now)
    {
        if (Status is DepositStatus.Closed)
        {
            throw new InvalidOperationException("Cannot pause closed deposit");
        }

        Status = DepositStatus.Paused;

        AddDomainEvent(new DepositPausedEvent(Id, now));
    }

    /// <summary>
    ///     Remove contribution from deposit.
    /// </summary>
    public void RemoveContribution(DepositContributionIdentity contributionId)
    {
        if (Status is not DepositStatus.Active)
        {
            throw new InvalidOperationException("Can only modify active deposit");
        }

        DepositContribution contribution = _contributions.FirstOrDefault(c => c.Id.Equals(contributionId))
            ?? throw new InvalidOperationException($"Contribution {contributionId.Value} not found");

        _contributions.Remove(contribution);
    }

    /// <summary>
    ///     Resume deposit monitoring.
    /// </summary>
    public void Resume(Instant now)
    {
        if (Status is not DepositStatus.Paused)
        {
            throw new InvalidOperationException("Can only resume paused deposit");
        }

        Status = DepositStatus.Active;

        AddDomainEvent(new DepositResumedEvent(Id, now));
    }

    /// <summary>
    ///     Update an existing contribution.
    /// </summary>
    public void UpdateContribution(
        DepositContributionIdentity contributionId,
        decimal? amount = null,
        Instant? contributedAt = null,
        decimal? exchangeRate = null)
    {
        if (Status is not DepositStatus.Active)
        {
            throw new InvalidOperationException("Can only modify active deposit");
        }

        DepositContribution contribution = _contributions.FirstOrDefault(c => c.Id.Equals(contributionId))
            ?? throw new InvalidOperationException($"Contribution {contributionId.Value} not found");

        contribution.Update(amount, contributedAt, exchangeRate);
    }

    /// <summary>
    ///     Update deposit details. Only allowed for manually created deposits.
    ///     API-imported deposits are read-only (except StopLoss).
    /// </summary>
    public void UpdateDetails(Instant? openedAt = null, Percentage? annualInterestRate = null, int? termMonths = null)
    {
        if (!CanEditDetails())
        {
            throw new InvalidOperationException("Cannot edit deposit imported from bank API. Only Stop-Loss threshold can be changed.");
        }

        if (openedAt is not null)
        {
            OpenedAt = openedAt.Value;
            MaturityDate = CalculateMaturityDate(openedAt.Value, TermMonths);
        }

        if (annualInterestRate is not null)
        {
            AnnualInterestRate = annualInterestRate;
        }

        if (termMonths is not null)
        {
            if (termMonths.Value <= 0)
            {
                throw new ArgumentException("Term must be positive", nameof(termMonths));
            }

            TermMonths = termMonths.Value;
            MaturityDate = CalculateMaturityDate(OpenedAt, termMonths.Value);
        }
    }

    /// <summary>
    ///     Update stop-loss threshold. Always allowed, even for API-imported deposits.
    /// </summary>
    public void UpdateStopLossThreshold(Percentage newThreshold)
    {
        StopLossThreshold = newThreshold;
    }

    private static LocalDate CalculateMaturityDate(Instant openedAt, int termMonths)
    {
        LocalDate openedDate = openedAt.InUtc().Date;

        return openedDate.PlusMonths(termMonths);
    }
}

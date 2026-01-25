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
        Money initialAmount,
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
        InitialAmount = initialAmount;
        CurrentAmount = initialAmount;
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
    ///     Additional contributions (pополнения).
    /// </summary>
    public IReadOnlyList<DepositContribution> Contributions => _contributions.AsReadOnly();

    /// <summary>
    ///     Current total amount (initial + contributions).
    /// </summary>
    public Money CurrentAmount { get; private set; } = null!;

    /// <summary>
    ///     Initial deposit amount.
    /// </summary>
    public Money InitialAmount { get; private set; } = null!;

    /// <summary>
    ///     Maturity date (calculated from OpenedAt + TermMonths).
    /// </summary>
    public Instant MaturityDate { get; private set; }

    /// <summary>
    ///     When deposit was opened.
    /// </summary>
    public Instant OpenedAt { get; private set; }

    /// <summary>
    ///     Exchange rate type (Commercial or Concept).
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
    ///     Factory method to create new deposit.
    /// </summary>
    public static Deposit Create(
        UserIdentity userId,
        BankIdentity bankId,
        Money initialAmount,
        Percentage annualInterestRate,
        Instant openedAt,
        int termMonths,
        RateType rateType,
        Percentage stopLossThreshold,
        DepositSource source,
        decimal initialExchangeRate)
    {
        ValidateCreationParameters(termMonths, initialExchangeRate);

        var deposit = new Deposit(
            DepositIdentity.New(),
            userId,
            bankId,
            initialAmount,
            annualInterestRate,
            openedAt,
            termMonths,
            rateType,
            stopLossThreshold,
            source);

        // Add initial contribution
        var initialContribution = DepositContribution.Create(initialAmount, openedAt, initialExchangeRate);

        deposit._contributions.Add(initialContribution);

        // Raise domain event
        deposit.AddDomainEvent(new DepositCreatedEvent(deposit.Id, userId, SystemClock.Instance.GetCurrentInstant()));

        return deposit;
    }

    /// <summary>
    ///     Add funds to deposit (пополнение).
    /// </summary>
    public void AddFunds(Money amount, decimal exchangeRate, Instant contributedAt)
    {
        if (Status is not DepositStatus.Active)
        {
            throw new InvalidOperationException("Can only add funds to active deposit");
        }

        if (!amount.HasSameCurrency(InitialAmount))
        {
            throw new InvalidOperationException(
                $"Contribution currency {amount.Currency} must match deposit currency {InitialAmount.Currency}");
        }

        if (exchangeRate <= 0)
        {
            throw new ArgumentException("Exchange rate must be positive", nameof(exchangeRate));
        }

        var contribution = DepositContribution.Create(amount, contributedAt, exchangeRate);
        _contributions.Add(contribution);

        CurrentAmount = CurrentAmount.Add(amount);

        AddDomainEvent(new DepositFundsAddedEvent(Id, amount, exchangeRate, SystemClock.Instance.GetCurrentInstant()));
    }

    /// <summary>
    ///     Calculate weighted average exchange rate across all contributions.
    ///     Used for profitability calculation when deposit has multiple contributions.
    /// </summary>
    public decimal CalculateWeightedAverageExchangeRate()
    {
        if (_contributions.Count is 0)
        {
            throw new InvalidOperationException("No contributions found");
        }

        if (_contributions.Count is 1)
        {
            return _contributions[0].ExchangeRateAtContribution;
        }

        Instant now = SystemClock.Instance.GetCurrentInstant();

        decimal totalWeighted = _contributions.Sum(c => c.Amount.Amount * c.ExchangeRateAtContribution * c.DaysFromContribution(now));

        decimal totalWeights = _contributions.Sum(c => c.Amount.Amount * c.DaysFromContribution(now));

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

        AddDomainEvent(new DepositClosedEvent(Id, closedAt, SystemClock.Instance.GetCurrentInstant()));
    }

    /// <summary>
    ///     Pause deposit monitoring.
    /// </summary>
    public void Pause()
    {
        if (Status is DepositStatus.Closed)
        {
            throw new InvalidOperationException("Cannot pause closed deposit");
        }

        Status = DepositStatus.Paused;

        AddDomainEvent(new DepositPausedEvent(Id, SystemClock.Instance.GetCurrentInstant()));
    }

    /// <summary>
    ///     Resume deposit monitoring.
    /// </summary>
    public void Resume()
    {
        if (Status is not DepositStatus.Paused)
        {
            throw new InvalidOperationException("Can only resume paused deposit");
        }

        Status = DepositStatus.Active;

        AddDomainEvent(new DepositResumedEvent(Id, SystemClock.Instance.GetCurrentInstant()));
    }

    /// <summary>
    ///     Update deposit details. Only allowed for manually created deposits.
    ///     API-imported deposits are read-only (except StopLoss).
    /// </summary>
    public void UpdateDetails(Instant? openedAt = null, Percentage? annualInterestRate = null, int? termMonths = null, Money? amount = null)
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

        if (amount is not null)
        {
            if (!amount.HasSameCurrency(InitialAmount))
            {
                throw new InvalidOperationException(
                    $"New amount currency {amount.Currency} must match deposit currency {InitialAmount.Currency}");
            }

            InitialAmount = amount;
            CurrentAmount = amount;
        }
    }

    /// <summary>
    ///     Update stop-loss threshold. Always allowed, even for API-imported deposits.
    /// </summary>
    public void UpdateStopLossThreshold(Percentage newThreshold)
    {
        StopLossThreshold = newThreshold;
    }

    private static Instant CalculateMaturityDate(Instant openedAt, int termMonths)
    {
        LocalDate openedDate = openedAt.InUtc().LocalDateTime.Date;
        LocalDate maturityDate = openedDate.PlusMonths(termMonths);

        return maturityDate.AtMidnight().InUtc().ToInstant();
    }

    private static void ValidateCreationParameters(int termMonths, decimal initialExchangeRate)
    {
        if (termMonths <= 0)
        {
            throw new ArgumentException("Term must be positive", nameof(termMonths));
        }

        if (initialExchangeRate <= 0)
        {
            throw new ArgumentException("Exchange rate must be positive", nameof(initialExchangeRate));
        }
    }
}

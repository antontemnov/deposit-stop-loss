using System;

using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Domain.Deposits;

/// <summary>
///     Entity representing a deposit contribution (initial or additional funds).
///     Used for weighted average profitability calculation.
/// </summary>
public sealed class DepositContribution : Entity<DepositContributionIdentity>
{
    private DepositContribution()
    {
    }

    private DepositContribution(
        DepositContributionIdentity id,
        DepositIdentity depositId,
        decimal amount,
        Instant contributedAt,
        decimal exchangeRateAtContribution)
        : base(id)
    {
        DepositId = depositId;
        Amount = amount;
        ContributedAt = contributedAt;
        ExchangeRateAtContribution = exchangeRateAtContribution;
    }

    /// <summary>
    ///     Amount contributed in deposit currency.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    ///     When funds were added.
    /// </summary>
    public Instant ContributedAt { get; private set; }

    /// <summary>
    ///     Parent deposit FK.
    /// </summary>
    public DepositIdentity DepositId { get; private set; }

    /// <summary>
    ///     Exchange rate at the time of contribution (e.g., 1 USD = 2.85 GEL).
    /// </summary>
    public decimal ExchangeRateAtContribution { get; private set; }

    /// <summary>
    ///     USD value at time of contribution (derived from Amount / ExchangeRate).
    /// </summary>
    public decimal ValueInUsd => Math.Round(Amount / ExchangeRateAtContribution, 2);

    /// <summary>
    ///     Calculate days from contribution to given point in time (for weighted average).
    /// </summary>
    public int DaysFromContribution(Instant now)
    {
        Duration duration = now - ContributedAt;

        return (int)duration.TotalDays + 1; // +1 to avoid zero weight
    }

    /// <summary>
    ///     Factory method to create contribution.
    /// </summary>
    internal static DepositContribution Create(DepositIdentity depositId, decimal amount, Instant contributedAt, decimal exchangeRate)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be positive", nameof(amount));
        }

        if (exchangeRate <= 0)
        {
            throw new ArgumentException("Exchange rate must be positive", nameof(exchangeRate));
        }

        return new DepositContribution(DepositContributionIdentity.New(), depositId, amount, contributedAt, exchangeRate);
    }

    /// <summary>
    ///     Update contribution details.
    /// </summary>
    internal void Update(decimal? amount = null, Instant? contributedAt = null, decimal? exchangeRate = null)
    {
        if (amount is not null)
        {
            if (amount.Value <= 0)
            {
                throw new ArgumentException("Amount must be positive", nameof(amount));
            }

            Amount = amount.Value;
        }

        if (contributedAt is not null)
        {
            ContributedAt = contributedAt.Value;
        }

        if (exchangeRate is not null)
        {
            if (exchangeRate.Value <= 0)
            {
                throw new ArgumentException("Exchange rate must be positive", nameof(exchangeRate));
            }

            ExchangeRateAtContribution = exchangeRate.Value;
        }
    }
}

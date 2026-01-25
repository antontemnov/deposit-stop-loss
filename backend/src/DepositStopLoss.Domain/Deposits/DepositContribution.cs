using System;

using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Domain.Deposits;

/// <summary>
///     Value Object representing a deposit contribution (initial or additional funds).
///     Used for weighted average profitability calculation.
/// </summary>
public sealed record DepositContribution
{
    private DepositContribution(Money amount, Instant contributedAt, decimal exchangeRateAtContribution, Money valueInUsd)
    {
        Amount = amount;
        ContributedAt = contributedAt;
        ExchangeRateAtContribution = exchangeRateAtContribution;
        ValueInUsd = valueInUsd;
    }

    /// <summary>
    ///     Amount contributed in deposit currency (GEL).
    /// </summary>
    public Money Amount { get; }

    /// <summary>
    ///     When funds were added.
    /// </summary>
    public Instant ContributedAt { get; }

    /// <summary>
    ///     Exchange rate at the time of contribution (e.g., 1 USD = 2.85 GEL).
    /// </summary>
    public decimal ExchangeRateAtContribution { get; }

    /// <summary>
    ///     USD value at time of contribution.
    /// </summary>
    public Money ValueInUsd { get; }

    /// <summary>
    ///     Factory method to create contribution.
    /// </summary>
    public static DepositContribution Create(Money amount, Instant contributedAt, decimal exchangeRate)
    {
        if (exchangeRate <= 0)
        {
            throw new ArgumentException("Exchange rate must be positive", nameof(exchangeRate));
        }

        Money usdValue = amount.ConvertTo(Currency.Usd, exchangeRate);

        return new DepositContribution(amount, contributedAt, exchangeRate, usdValue);
    }

    /// <summary>
    ///     Calculate days from contribution to now (for weighted average).
    /// </summary>
    public int DaysFromContribution(Instant now)
    {
        Duration duration = now - ContributedAt;

        return (int)duration.TotalDays + 1; // +1 to avoid zero
    }
}

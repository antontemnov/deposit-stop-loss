using System;

using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Domain.Snapshots;

/// <summary>
///     Deposit snapshot entity. Historical record of deposit profitability at a point in time.
/// </summary>
public sealed class DepositSnapshot : Entity<DepositSnapshotIdentity>
{
    private DepositSnapshot()
    {
    }

    private DepositSnapshot(
        DepositSnapshotIdentity id,
        DepositIdentity depositId,
        Money totalAmountInDepositCurrency,
        Money totalAmountInUsd,
        Money accruedInterest,
        Percentage profitLossPercent,
        decimal exchangeRate,
        Instant snapshotDate)
        : base(id)
    {
        DepositId = depositId;
        TotalAmountInDepositCurrency = totalAmountInDepositCurrency;
        TotalAmountInUsd = totalAmountInUsd;
        AccruedInterest = accruedInterest;
        ProfitLossPercent = profitLossPercent;
        ExchangeRate = exchangeRate;
        SnapshotDate = snapshotDate;
    }

    /// <summary>
    ///     Accrued interest amount in deposit currency.
    /// </summary>
    public Money AccruedInterest { get; private set; } = null!;

    /// <summary>
    ///     Related deposit.
    /// </summary>
    public DepositIdentity DepositId { get; private set; }

    /// <summary>
    ///     Exchange rate used for conversion (USD/GEL).
    /// </summary>
    public decimal ExchangeRate { get; private set; }

    /// <summary>
    ///     Profit/Loss percentage in USD compared to initial investment.
    /// </summary>
    public Percentage ProfitLossPercent { get; private set; } = null!;

    /// <summary>
    ///     When this snapshot was taken.
    /// </summary>
    public Instant SnapshotDate { get; private set; }

    /// <summary>
    ///     Total amount in deposit currency (GEL) including accrued interest.
    /// </summary>
    public Money TotalAmountInDepositCurrency { get; private set; } = null!;

    /// <summary>
    ///     Total amount converted to USD at current exchange rate.
    /// </summary>
    public Money TotalAmountInUsd { get; private set; } = null!;

    /// <summary>
    ///     Factory method to create snapshot.
    /// </summary>
    public static DepositSnapshot Create(
        DepositIdentity depositId,
        Money totalAmountInDepositCurrency,
        Money totalAmountInUsd,
        Money accruedInterest,
        Percentage profitLossPercent,
        decimal exchangeRate,
        Instant snapshotDate)
    {
        if (exchangeRate <= 0)
        {
            throw new ArgumentException("Exchange rate must be positive", nameof(exchangeRate));
        }

        return new DepositSnapshot(
            DepositSnapshotIdentity.New(),
            depositId,
            totalAmountInDepositCurrency,
            totalAmountInUsd,
            accruedInterest,
            profitLossPercent,
            exchangeRate,
            snapshotDate);
    }
}

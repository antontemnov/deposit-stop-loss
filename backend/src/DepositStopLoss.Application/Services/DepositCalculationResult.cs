using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Application.Services;

/// <summary>
///     Result of deposit calculation.
/// </summary>
public sealed record DepositCalculationResult
{
    public required Money AccruedInterest { get; init; }

    public required Instant CalculatedAt { get; init; }

    public required decimal ExchangeRate { get; init; }

    /// <summary>
    ///     Notification level if should notify.
    /// </summary>
    public NotificationLevel? NotificationLevel { get; init; }

    public required Percentage ProfitLossPercent { get; init; }

    /// <summary>
    ///     Should user be notified based on profitability and threshold.
    /// </summary>
    public required bool ShouldNotify { get; init; }

    public required Money TotalAmountInDepositCurrency { get; init; }

    public required Money TotalAmountInUsd { get; init; }
}

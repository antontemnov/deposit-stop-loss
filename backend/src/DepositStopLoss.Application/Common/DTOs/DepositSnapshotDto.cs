using System;

using NodaTime;

namespace DepositStopLoss.Application.Common.DTOs;

/// <summary>
///     DTO for DepositSnapshot entity.
/// </summary>
public sealed record DepositSnapshotDto
{
    public required decimal AccruedInterest { get; init; }

    public required string AccruedInterestCurrency { get; init; }

    public required Guid DepositId { get; init; }

    public required decimal ExchangeRate { get; init; }

    public required Guid Id { get; init; }

    public required decimal ProfitLossPercent { get; init; }

    public required Instant SnapshotDate { get; init; }

    public required decimal TotalAmountInDepositCurrency { get; init; }

    public required decimal TotalAmountInUsd { get; init; }
}

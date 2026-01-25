using System;
using System.Collections.Generic;

using NodaTime;

namespace DepositStopLoss.Application.Common.DTOs;

/// <summary>
///     DTO for Deposit entity.
/// </summary>
public sealed record DepositDto
{
    public required decimal AnnualInterestRate { get; init; }

    public required Guid BankId { get; init; }

    public Instant? ClosedAt { get; init; }

    public List<DepositContributionDto>? Contributions { get; init; }

    public required string Currency { get; init; }

    public required decimal CurrentAmount { get; init; }

    public required Guid Id { get; init; }

    public required decimal InitialAmount { get; init; }

    public required Instant MaturityDate { get; init; }

    public required Instant OpenedAt { get; init; }

    public required string RateType { get; init; }

    public required string Source { get; init; }

    public required string Status { get; init; }

    public required decimal StopLossThreshold { get; init; }

    public required int TermMonths { get; init; }

    public required Guid UserId { get; init; }
}

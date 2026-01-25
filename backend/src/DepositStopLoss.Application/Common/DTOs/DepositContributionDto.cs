using NodaTime;

namespace DepositStopLoss.Application.Common.DTOs;

/// <summary>
///     DTO for Deposit Contribution.
/// </summary>
public sealed record DepositContributionDto
{
    public required decimal Amount { get; init; }

    public required Instant ContributedAt { get; init; }

    public required string Currency { get; init; }

    public required decimal ExchangeRate { get; init; }

    public required decimal ValueInUsd { get; init; }
}

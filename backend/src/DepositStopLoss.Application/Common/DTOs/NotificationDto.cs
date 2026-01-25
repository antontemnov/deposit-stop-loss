using System;

using NodaTime;

namespace DepositStopLoss.Application.Common.DTOs;

/// <summary>
///     DTO for Notification entity.
/// </summary>
public sealed record NotificationDto
{
    public required Instant CreatedAt { get; init; }

    public required Guid DepositId { get; init; }

    public string? ErrorMessage { get; init; }

    public required Guid Id { get; init; }

    public required string Level { get; init; }

    public required string Message { get; init; }

    public Instant? ReadAt { get; init; }

    public Instant? SentAt { get; init; }

    public required string Status { get; init; }

    public required Guid UserId { get; init; }
}

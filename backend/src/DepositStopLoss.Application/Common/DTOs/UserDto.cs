using System;

using NodaTime;

namespace DepositStopLoss.Application.Common.DTOs;

/// <summary>
///     DTO for User entity.
/// </summary>
public sealed record UserDto
{
    public string? Email { get; init; }

    public required bool EnableEmailNotifications { get; init; }

    public required bool EnableTelegramNotifications { get; init; }

    public required string FirstName { get; init; }

    public required Guid Id { get; init; }

    public required bool IsActive { get; init; }

    public required string LanguageCode { get; init; }

    public Instant? LastActivityAt { get; init; }

    public string? LastName { get; init; }

    public required Instant RegisteredAt { get; init; }

    public required long TelegramId { get; init; }

    public required string Timezone { get; init; }

    public string? Username { get; init; }
}

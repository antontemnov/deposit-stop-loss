using System;

namespace DepositStopLoss.Application.Common.DTOs;

/// <summary>
///     DTO for Bank entity.
/// </summary>
public sealed record BankDto
{
    public required string Code { get; init; }

    public required Guid Id { get; init; }

    public required bool IsActive { get; init; }

    public required string Name { get; init; }
}

using System;

namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Marker interface for domain events.
///     Application layer will wrap this for MediatR integration.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}

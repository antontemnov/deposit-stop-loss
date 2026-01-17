using System;

namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Base record for all domain events.
///     Using record for immutability and value-based equality.
/// </summary>
public abstract record DomainEventBase : IDomainEvent
{
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}

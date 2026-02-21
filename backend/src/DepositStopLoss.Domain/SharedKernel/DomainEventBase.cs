using NodaTime;

namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Base record for all domain events.
///     Using record for immutability and value-based equality.
/// </summary>
public abstract record DomainEventBase(Instant OccurredAt) : IDomainEvent;

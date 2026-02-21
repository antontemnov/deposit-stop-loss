using NodaTime;

namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Marker interface for domain events.
/// </summary>
public interface IDomainEvent
{
    Instant OccurredAt { get; }
}

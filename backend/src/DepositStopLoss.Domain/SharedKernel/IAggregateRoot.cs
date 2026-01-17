using System.Collections.Generic;

namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Marker interface for aggregate roots.
///     Aggregates are consistency boundaries in DDD.
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    ///     Domain events raised by aggregate.
    /// </summary>
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
}

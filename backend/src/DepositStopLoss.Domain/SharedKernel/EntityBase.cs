using System.Collections.Generic;

namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Base class for all entities with domain events support.
/// </summary>
public abstract class EntityBase
{
    private readonly List<IDomainEvent> _domainEvents =
    [
    ];

    /// <summary>
    ///     Domain events raised by this entity.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    ///     Clears all domain events (called after dispatching).
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    ///     Adds a domain event to be dispatched.
    /// </summary>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}

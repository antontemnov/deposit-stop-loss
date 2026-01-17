using System.Collections.Generic;

namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Base class for all entities with domain events support.
/// </summary>
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    ///     Domain events raised by this entity.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    ///     Adds a domain event to be dispatched.
    /// </summary>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    ///     Clears all domain events (called after dispatching).
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

/// <summary>
///     Base class for entities with strongly typed identity.
/// </summary>
/// <typeparam name="TIdentity">Strongly typed ID type</typeparam>
public abstract class Entity<TIdentity> : Entity where TIdentity : struct
{
    protected Entity() { }

    protected Entity(TIdentity id)
    {
        Id = id;
    }

    public TIdentity Id { get; protected set; }
}

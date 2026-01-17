namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Base class for aggregate roots with strongly typed identity.
/// </summary>
/// <typeparam name="TIdentity">Strongly typed ID type</typeparam>
public abstract class AggregateRoot<TIdentity> : Entity<TIdentity>, IAggregateRoot
    where TIdentity : struct
{
    protected AggregateRoot() { }

    protected AggregateRoot(TIdentity id) : base(id)
    {
    }
}

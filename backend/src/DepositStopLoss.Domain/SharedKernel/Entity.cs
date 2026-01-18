namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Base class for entities with strongly typed identity.
/// </summary>
/// <typeparam name="TIdentity">Strongly typed ID type</typeparam>
public abstract class Entity<TIdentity> : EntityBase
    where TIdentity : struct
{
    protected Entity()
    {
    }

    protected Entity(TIdentity id)
    {
        Id = id;
    }

    public TIdentity Id { get; protected set; }
}

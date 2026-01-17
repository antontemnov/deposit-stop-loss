using System;

namespace DepositStopLoss.Domain.Users;

/// <summary>
///     Strongly typed ID for User aggregate root.
///     Prevents accidental ID mix-ups at compile time.
/// </summary>
public readonly record struct UserIdentity(Guid Value)
{
    public static UserIdentity Empty => new(Guid.Empty);

    public static UserIdentity New()
    {
        return new UserIdentity(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

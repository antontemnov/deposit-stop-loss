using System;

namespace DepositStopLoss.Domain.Banking;

/// <summary>
///     Strongly typed ID for UserBankConnection entity (OAuth connections).
///     Prevents accidental ID mix-ups at compile time.
/// </summary>
public readonly record struct UserBankConnectionIdentity(Guid Value)
{
    public static UserBankConnectionIdentity Empty => new(Guid.Empty);

    public static UserBankConnectionIdentity New()
    {
        return new UserBankConnectionIdentity(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

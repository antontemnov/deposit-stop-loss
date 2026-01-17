using System;

namespace DepositStopLoss.Domain.Banking;

/// <summary>
///     Strongly typed ID for Bank entity.
///     Prevents accidental ID mix-ups at compile time.
/// </summary>
public readonly record struct BankIdentity(Guid Value)
{
    public static BankIdentity Empty => new(Guid.Empty);

    public static BankIdentity New()
    {
        return new BankIdentity(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

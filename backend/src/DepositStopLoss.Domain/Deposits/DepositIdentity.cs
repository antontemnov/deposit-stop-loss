using System;

namespace DepositStopLoss.Domain.Deposits;

/// <summary>
///     Strongly typed ID for Deposit aggregate root.
///     Prevents accidental ID mix-ups at compile time.
/// </summary>
public readonly record struct DepositIdentity(Guid Value)
{
    public static DepositIdentity Empty => new(Guid.Empty);

    public static DepositIdentity New()
    {
        return new DepositIdentity(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

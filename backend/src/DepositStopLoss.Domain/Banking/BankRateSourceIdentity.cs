using System;

namespace DepositStopLoss.Domain.Banking;

/// <summary>
///     Strongly typed ID for BankRateSource entity.
/// </summary>
public readonly record struct BankRateSourceIdentity(Guid Value)
{
    public static BankRateSourceIdentity Empty => new(Guid.Empty);

    public static BankRateSourceIdentity New()
    {
        return new BankRateSourceIdentity(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

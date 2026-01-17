using System;

namespace DepositStopLoss.Domain.Banking;

/// <summary>
///     Strongly typed ID for ExchangeRate entity.
///     Prevents accidental ID mix-ups at compile time.
/// </summary>
public readonly record struct ExchangeRateIdentity(Guid Value)
{
    public static ExchangeRateIdentity Empty => new(Guid.Empty);

    public static ExchangeRateIdentity New()
    {
        return new ExchangeRateIdentity(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

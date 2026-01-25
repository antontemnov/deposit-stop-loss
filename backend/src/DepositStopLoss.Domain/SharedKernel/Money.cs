using System;

namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Value Object representing monetary amount with currency.
///     Immutable, value-based equality.
/// </summary>
public sealed record Money
{
    private Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public decimal Amount { get; }

    public Currency Currency { get; }

    /// <summary>
    ///     Factory method with validation.
    /// </summary>
    public static Money Create(decimal amount, Currency currency)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        }

        return new Money(Math.Round(amount, 2), currency);
    }

    public static Money Eur(decimal amount)
    {
        return Create(amount, Currency.Eur);
    }

    // Common currencies shortcuts
    public static Money Gel(decimal amount)
    {
        return Create(amount, Currency.Gel);
    }

    public static Money Usd(decimal amount)
    {
        return Create(amount, Currency.Usd);
    }

    public static Money Zero(Currency currency)
    {
        return Create(0, currency);
    }

    /// <summary>
    ///     Adds two Money values. Must be same currency.
    /// </summary>
    public Money Add(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);
        EnsureSameCurrency(other);

        return Create(Amount + other.Amount, Currency);
    }

    /// <summary>
    ///     Divides amount by exchange rate to convert to another currency.
    ///     Example: 100 GEL / 2.85 = 35.09 USD
    /// </summary>
    public Money ConvertTo(Currency targetCurrency, decimal exchangeRate)
    {
        if (exchangeRate <= 0)
        {
            throw new ArgumentException("Exchange rate must be positive", nameof(exchangeRate));
        }

        decimal convertedAmount = Math.Round(Amount / exchangeRate, 2);

        return Create(convertedAmount, targetCurrency);
    }

    /// <summary>
    ///     Checks if this Money has same currency as other.
    /// </summary>
    public bool HasSameCurrency(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);

        return Currency == other.Currency;
    }

    /// <summary>
    ///     Multiplies amount by a factor (e.g., for interest calculation).
    /// </summary>
    public Money MultiplyBy(decimal factor)
    {
        decimal result = Amount * factor;

        return result >= 0 ? Create(result, Currency) : CreateAllowNegative(result, Currency);
    }

    /// <summary>
    ///     Subtracts other Money from this. Must be same currency.
    ///     Result can be negative (for loss calculations).
    /// </summary>
    public Money Subtract(Money other)
    {
        ArgumentNullException.ThrowIfNull(other);
        EnsureSameCurrency(other);

        return CreateAllowNegative(Amount - other.Amount, Currency);
    }

    public override string ToString()
    {
        return $"{Amount:N2} {Currency}";
    }

    /// <summary>
    ///     Internal factory allowing negative amounts (for loss calculations).
    /// </summary>
    private static Money CreateAllowNegative(decimal amount, Currency currency)
    {
        return new Money(Math.Round(amount, 2), currency);
    }

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
        {
            throw new InvalidOperationException(
                $"Cannot perform operation on Money with different currencies: {Currency} and {other.Currency}");
        }
    }
}

using System;

namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Value Object representing percentage value.
///     Stores as decimal (e.g., 12.5 means 12.5%, not 0.125).
/// </summary>
public sealed record Percentage
{
    // EF Core ValueConverter requires parameterless ctor
    private Percentage()
    {
    }

    private Percentage(decimal value)
    {
        Value = value;
    }

    public static Percentage Zero => new(0);

    /// <summary>
    ///     Checks if percentage is negative (loss).
    /// </summary>
    public bool IsNegative => Value < 0;

    public decimal Value { get; }

    /// <summary>
    ///     Creates percentage from fraction (0.125 = 12.5%).
    /// </summary>
    public static Percentage FromFraction(decimal fraction)
    {
        return new Percentage(Math.Round(fraction * 100, 2));
    }

    /// <summary>
    ///     Creates percentage from direct value (12.5 = 12.5%).
    /// </summary>
    public static Percentage FromValue(decimal value)
    {
        return new Percentage(Math.Round(value, 2));
    }

    /// <summary>
    ///     Checks if percentage is below threshold.
    /// </summary>
    public bool IsBelowThreshold(Percentage threshold)
    {
        ArgumentNullException.ThrowIfNull(threshold);

        return Value <= threshold.Value;
    }

    /// <summary>
    ///     Converts to fraction for calculations (12.5% -> 0.125).
    /// </summary>
    public decimal ToFraction()
    {
        return Value / 100;
    }

    public override string ToString()
    {
        return $"{Value:N2}%";
    }
}

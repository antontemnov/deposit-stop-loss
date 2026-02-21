namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Type of exchange rate used for deposit profitability calculation.
/// </summary>
public enum RateType
{
    /// <summary>
    ///     Unknown/unspecified rate type
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     Standard commercial bank rates
    /// </summary>
    Commercial = 1,

    /// <summary>
    ///     Discounted rates (loyalty programs, premium subscriptions, etc.)
    /// </summary>
    Discounted = 2,
}

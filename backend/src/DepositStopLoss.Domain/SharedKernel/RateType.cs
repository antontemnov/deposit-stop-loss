namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Type of exchange rate (commercial vs discounted for TBC Concept users).
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
    ///     Discounted rates for TBC Concept subscription holders
    /// </summary>
    Concept = 2
}

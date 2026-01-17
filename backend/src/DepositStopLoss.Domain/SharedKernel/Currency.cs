namespace DepositStopLoss.Domain.SharedKernel;

/// <summary>
///     Supported currencies (ISO 4217).
///     GEL is primary deposit currency, USD is comparison base.
/// </summary>
public enum Currency
{
    /// <summary>
    ///     Unknown/unspecified currency
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     Georgian Lari - primary deposit currency
    /// </summary>
    Gel = 1,

    /// <summary>
    ///     US Dollar - comparison base currency
    /// </summary>
    Usd = 2,

    /// <summary>
    ///     Euro - future support
    /// </summary>
    Eur = 3
}

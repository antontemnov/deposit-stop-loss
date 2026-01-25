namespace DepositStopLoss.Domain.Deposits;

/// <summary>
///     Source of deposit data. Affects editability rules.
/// </summary>
public enum DepositSource
{
    /// <summary>
    ///     Unknown/unspecified source
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     User entered deposit manually - fully editable
    /// </summary>
    Manual = 1,

    /// <summary>
    ///     Imported via TBC Bank API (OAuth) - readonly except StopLoss
    /// </summary>
    TbcApi = 2,

    /// <summary>
    ///     Imported via Bank of Georgia API - readonly except StopLoss
    /// </summary>
    BogApi = 3,
}

namespace DepositStopLoss.Domain.Banking;

/// <summary>
///     Type of bank in the system.
/// </summary>
public enum BankType
{
    /// <summary>
    ///     Unknown/unspecified bank type
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     Commercial bank that accepts deposits (TBC, BOG)
    /// </summary>
    Commercial = 1,

    /// <summary>
    ///     Central/national bank used only as exchange rate source (NBG)
    /// </summary>
    CentralBank = 2,
}

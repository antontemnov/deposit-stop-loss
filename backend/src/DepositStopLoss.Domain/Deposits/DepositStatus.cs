namespace DepositStopLoss.Domain.Deposits;

/// <summary>
///     Status of a deposit in its lifecycle.
/// </summary>
public enum DepositStatus
{
    /// <summary>
    ///     Unknown/unspecified status
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     Deposit is actively monitored
    /// </summary>
    Active = 1,

    /// <summary>
    ///     Monitoring is temporarily paused by user
    /// </summary>
    Paused = 2,

    /// <summary>
    ///     Deposit has been closed/withdrawn
    /// </summary>
    Closed = 3,
}

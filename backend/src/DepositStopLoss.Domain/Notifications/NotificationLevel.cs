namespace DepositStopLoss.Domain.Notifications;

/// <summary>
///     Severity level of notification. Affects cooldown period.
/// </summary>
public enum NotificationLevel
{
    /// <summary>
    ///     Unknown/unspecified level
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     Profitability below expected rate. Cooldown: 24h
    /// </summary>
    Info = 1,

    /// <summary>
    ///     Approaching stop-loss threshold (2-4% away). Cooldown: 12h
    /// </summary>
    Warning = 2,

    /// <summary>
    ///     Profitability is zero or negative. Cooldown: 1h
    /// </summary>
    Critical = 3,
}

namespace DepositStopLoss.Domain.Notifications;

/// <summary>
///     Delivery status of a notification.
/// </summary>
public enum NotificationStatus
{
    /// <summary>
    ///     Unknown/unspecified status
    /// </summary>
    Unknown = 0,

    /// <summary>
    ///     Notification created, waiting to be sent
    /// </summary>
    Pending = 1,

    /// <summary>
    ///     Successfully delivered to user
    /// </summary>
    Sent = 2,

    /// <summary>
    ///     Delivery failed (will retry)
    /// </summary>
    Failed = 3,

    /// <summary>
    ///     User acknowledged/read the notification
    /// </summary>
    Read = 4,
}

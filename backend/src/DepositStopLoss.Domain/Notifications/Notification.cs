using System;

using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.SharedKernel;
using DepositStopLoss.Domain.Users;

using NodaTime;

namespace DepositStopLoss.Domain.Notifications;

/// <summary>
///     Notification entity. Represents notification to be sent to user.
/// </summary>
public sealed class Notification : Entity<NotificationIdentity>
{
    private Notification()
    {
    }

    private Notification(
        NotificationIdentity id,
        UserIdentity userId,
        DepositIdentity depositId,
        NotificationLevel level,
        string message,
        Instant createdAt)
        : base(id)
    {
        UserId = userId;
        DepositId = depositId;
        Level = level;
        Message = message;
        Status = NotificationStatus.Pending;
        CreatedAt = createdAt;
    }

    /// <summary>
    ///     When notification was created.
    /// </summary>
    public Instant CreatedAt { get; private set; }

    /// <summary>
    ///     Related deposit.
    /// </summary>
    public DepositIdentity DepositId { get; private set; }

    /// <summary>
    ///     Error message if sending failed.
    /// </summary>
    public string? ErrorMessage { get; private set; }

    /// <summary>
    ///     Notification severity level.
    /// </summary>
    public NotificationLevel Level { get; private set; }

    /// <summary>
    ///     Notification message.
    /// </summary>
    public string Message { get; private set; } = null!;

    /// <summary>
    ///     When user read notification (null if not read).
    /// </summary>
    public Instant? ReadAt { get; private set; }

    /// <summary>
    ///     When notification was sent (null if not sent yet).
    /// </summary>
    public Instant? SentAt { get; private set; }

    /// <summary>
    ///     Notification status (Pending, Sent, Failed, Read).
    /// </summary>
    public NotificationStatus Status { get; private set; }

    /// <summary>
    ///     User to notify.
    /// </summary>
    public UserIdentity UserId { get; private set; }

    /// <summary>
    ///     Factory method to create notification.
    /// </summary>
    public static Notification Create(
        UserIdentity userId,
        DepositIdentity depositId,
        NotificationLevel level,
        string message,
        Instant createdAt)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Message cannot be empty", nameof(message));
        }

        return new Notification(NotificationIdentity.New(), userId, depositId, level, message, createdAt);
    }

    /// <summary>
    ///     Mark notification as failed.
    /// </summary>
    public void MarkAsFailed(string errorMessage)
    {
        Status = NotificationStatus.Failed;
        ErrorMessage = errorMessage;
    }

    /// <summary>
    ///     Mark notification as read by user.
    /// </summary>
    public void MarkAsRead(Instant readAt)
    {
        if (Status is not NotificationStatus.Sent)
        {
            throw new InvalidOperationException("Can only mark sent notifications as read");
        }

        Status = NotificationStatus.Read;
        ReadAt = readAt;
    }

    /// <summary>
    ///     Mark notification as sent.
    /// </summary>
    public void MarkAsSent(Instant sentAt)
    {
        if (Status is NotificationStatus.Sent)
        {
            return; // Already sent
        }

        Status = NotificationStatus.Sent;
        SentAt = sentAt;
        ErrorMessage = null;
    }

    /// <summary>
    ///     Retry sending failed notification.
    /// </summary>
    public void RetrySending()
    {
        if (Status is not NotificationStatus.Failed)
        {
            throw new InvalidOperationException("Can only retry failed notifications");
        }

        Status = NotificationStatus.Pending;
        ErrorMessage = null;
    }
}

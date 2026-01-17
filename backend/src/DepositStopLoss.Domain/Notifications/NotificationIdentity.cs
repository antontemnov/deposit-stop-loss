using System;

namespace DepositStopLoss.Domain.Notifications;

/// <summary>
///     Strongly typed ID for Notification entity.
///     Prevents accidental ID mix-ups at compile time.
/// </summary>
public readonly record struct NotificationIdentity(Guid Value)
{
    public static NotificationIdentity Empty => new(Guid.Empty);

    public static NotificationIdentity New()
    {
        return new NotificationIdentity(Guid.NewGuid());
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

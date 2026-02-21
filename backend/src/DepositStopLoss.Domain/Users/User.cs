using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Domain.Users;

/// <summary>
///     User aggregate root. Represents a registered user.
/// </summary>
public sealed class User : AggregateRoot<UserIdentity>
{
    private User()
    {
    }

    private User(UserIdentity id, long telegramId, string username, string firstName, string languageCode, Instant registeredAt)
        : base(id)
    {
        TelegramId = telegramId;
        Username = username;
        FirstName = firstName;
        LanguageCode = languageCode;
        RegisteredAt = registeredAt;
        IsActive = true;
        EnableTelegramNotifications = true;
        Timezone = "Asia/Tbilisi"; // default for Georgia
    }

    /// <summary>
    ///     Email for notifications (optional).
    /// </summary>
    public string? Email { get; private set; }

    /// <summary>
    ///     Enable email notifications.
    /// </summary>
    public bool EnableEmailNotifications { get; private set; }

    /// <summary>
    ///     Enable Telegram notifications.
    /// </summary>
    public bool EnableTelegramNotifications { get; private set; }

    /// <summary>
    ///     User's first name from Telegram.
    /// </summary>
    public string FirstName { get; private set; } = null!;

    /// <summary>
    ///     Is user account active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    ///     Preferred language code (en, ru, ka).
    /// </summary>
    public string LanguageCode { get; private set; } = null!;

    /// <summary>
    ///     Last activity timestamp.
    /// </summary>
    public Instant? LastActivityAt { get; private set; }

    /// <summary>
    ///     User's last name from Telegram (optional).
    /// </summary>
    public string? LastName { get; private set; }

    /// <summary>
    ///     When user registered.
    /// </summary>
    public Instant RegisteredAt { get; private set; }

    /// <summary>
    ///     Unique Telegram user ID.
    /// </summary>
    public long TelegramId { get; private set; }

    /// <summary>
    ///     IANA timezone identifier (e.g., "Asia/Tbilisi").
    /// </summary>
    public string Timezone { get; private set; } = null!;

    /// <summary>
    ///     Telegram username (optional).
    /// </summary>
    public string? Username { get; private set; }

    /// <summary>
    ///     Factory method to create new user from Telegram registration.
    /// </summary>
    public static User Register(
        long telegramId,
        string username,
        string firstName,
        string? lastName,
        string languageCode,
        Instant registeredAt)
    {
        var user = new User(UserIdentity.New(), telegramId, username, firstName, languageCode, registeredAt);

        user.LastName = lastName;

        return user;
    }

    /// <summary>
    ///     Reactivate user account.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }

    /// <summary>
    ///     Deactivate user account.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    ///     Update last activity timestamp.
    /// </summary>
    public void RecordActivity(Instant now)
    {
        LastActivityAt = now;
    }

    /// <summary>
    ///     Update user settings.
    /// </summary>
    public void UpdateSettings(string? timezone = null, string? email = null, bool? enableTelegram = null, bool? enableEmail = null)
    {
        if (timezone is not null)
        {
            Timezone = timezone;
        }

        if (email is not null)
        {
            Email = email;
        }

        if (enableTelegram.HasValue)
        {
            EnableTelegramNotifications = enableTelegram.Value;
        }

        if (enableEmail.HasValue)
        {
            EnableEmailNotifications = enableEmail.Value;
        }
    }
}

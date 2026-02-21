using System;

using DepositStopLoss.Domain.SharedKernel;
using DepositStopLoss.Domain.Users;

using NodaTime;

namespace DepositStopLoss.Domain.Banking;

/// <summary>
///     User bank connection entity. Stores OAuth connection to bank API.
/// </summary>
public sealed class UserBankConnection : Entity<UserBankConnectionIdentity>
{
    private UserBankConnection()
    {
    }

    private UserBankConnection(
        UserBankConnectionIdentity id,
        UserIdentity userId,
        BankIdentity bankId,
        string accessToken,
        string refreshToken,
        Instant tokenExpiresAt,
        string[] scopes,
        Instant connectedAt)
        : base(id)
    {
        UserId = userId;
        BankId = bankId;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        TokenExpiresAt = tokenExpiresAt;
        Scopes = scopes;
        ConnectedAt = connectedAt;
        IsActive = true;
    }

    /// <summary>
    ///     OAuth access token (encrypted in DB).
    /// </summary>
    public string AccessToken { get; private set; } = null!;

    /// <summary>
    ///     Connected bank.
    /// </summary>
    public BankIdentity BankId { get; private set; }

    /// <summary>
    ///     When user connected the bank.
    /// </summary>
    public Instant ConnectedAt { get; private set; }

    /// <summary>
    ///     Is connection active.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    ///     Last successful sync with bank API.
    /// </summary>
    public Instant? LastSyncAt { get; private set; }

    /// <summary>
    ///     OAuth refresh token (encrypted in DB).
    /// </summary>
    public string RefreshToken { get; private set; } = null!;

    /// <summary>
    ///     OAuth scopes granted (e.g., "savings:read").
    /// </summary>
    public string[] Scopes { get; private set; } = null!;

    /// <summary>
    ///     When access token expires.
    /// </summary>
    public Instant TokenExpiresAt { get; private set; }

    /// <summary>
    ///     User who connected the bank.
    /// </summary>
    public UserIdentity UserId { get; private set; }

    /// <summary>
    ///     Factory method to create bank connection.
    /// </summary>
    public static UserBankConnection Create(
        UserIdentity userId,
        BankIdentity bankId,
        string accessToken,
        string refreshToken,
        Instant tokenExpiresAt,
        string[] scopes,
        Instant connectedAt)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new ArgumentException("Access token cannot be empty", nameof(accessToken));
        }

        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new ArgumentException("Refresh token cannot be empty", nameof(refreshToken));
        }

        return new UserBankConnection(
            UserBankConnectionIdentity.New(),
            userId,
            bankId,
            accessToken,
            refreshToken,
            tokenExpiresAt,
            scopes,
            connectedAt);
    }

    /// <summary>
    ///     Disconnect bank.
    /// </summary>
    public void Disconnect()
    {
        IsActive = false;
    }

    /// <summary>
    ///     Check if access token is expired.
    /// </summary>
    public bool IsTokenExpired(Instant now)
    {
        return now >= TokenExpiresAt;
    }

    /// <summary>
    ///     Reconnect bank.
    /// </summary>
    public void Reconnect()
    {
        IsActive = true;
    }

    /// <summary>
    ///     Record successful sync.
    /// </summary>
    public void RecordSync(Instant syncedAt)
    {
        LastSyncAt = syncedAt;
    }

    /// <summary>
    ///     Update tokens after refresh.
    /// </summary>
    public void UpdateTokens(string newAccessToken, string newRefreshToken, Instant newExpiresAt)
    {
        AccessToken = newAccessToken;
        RefreshToken = newRefreshToken;
        TokenExpiresAt = newExpiresAt;
    }
}

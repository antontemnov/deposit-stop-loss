using System;

using DepositStopLoss.Domain.SharedKernel;

namespace DepositStopLoss.Domain.Banking;

/// <summary>
///     Defines an exchange rate source for a specific bank and rate type.
///     Each bank can support multiple rate types (e.g., commercial, discounted).
/// </summary>
public sealed class BankRateSource : Entity<BankRateSourceIdentity>
{
    private BankRateSource()
    {
    }

    private BankRateSource(BankRateSourceIdentity id, BankIdentity bankId, RateType rateType, string apiUrl)
        : base(id)
    {
        BankId = bankId;
        RateType = rateType;
        ApiUrl = apiUrl;
    }

    /// <summary>
    ///     API endpoint URL for fetching exchange rates.
    /// </summary>
    public string ApiUrl { get; private set; } = null!;

    /// <summary>
    ///     Owning bank.
    /// </summary>
    public BankIdentity BankId { get; private set; }

    /// <summary>
    ///     Type of rate this source provides.
    /// </summary>
    public RateType RateType { get; private set; }

    /// <summary>
    ///     Factory method.
    /// </summary>
    public static BankRateSource Create(BankIdentity bankId, RateType rateType, string apiUrl)
    {
        if (string.IsNullOrWhiteSpace(apiUrl))
        {
            throw new ArgumentException("API URL cannot be empty", nameof(apiUrl));
        }

        if (rateType is RateType.Unknown)
        {
            throw new ArgumentException("Rate type must be specified", nameof(rateType));
        }

        return new BankRateSource(BankRateSourceIdentity.New(), bankId, rateType, apiUrl);
    }

    /// <summary>
    ///     Update the API URL.
    /// </summary>
    public void UpdateApiUrl(string apiUrl)
    {
        if (string.IsNullOrWhiteSpace(apiUrl))
        {
            throw new ArgumentException("API URL cannot be empty", nameof(apiUrl));
        }

        ApiUrl = apiUrl;
    }
}

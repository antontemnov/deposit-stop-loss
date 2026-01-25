using DepositStopLoss.Domain.SharedKernel;

namespace DepositStopLoss.Domain.Banking;

/// <summary>
///     Bank entity. Represents supported banks (TBC, BOG, etc.).
/// </summary>
public sealed class Bank : Entity<BankIdentity>
{
    private Bank()
    {
    }

    private Bank(BankIdentity id, string code, string name, string commercialApiUrl, string? discountedApiUrl)
        : base(id)
    {
        Code = code;
        Name = name;
        CommercialApiUrl = commercialApiUrl;
        DiscountedApiUrl = discountedApiUrl;
        IsActive = true;
    }

    /// <summary>
    ///     Bank code (TBC, BOG, NBG).
    /// </summary>
    public string Code { get; private set; } = null!;

    /// <summary>
    ///     API URL for commercial exchange rates.
    /// </summary>
    public string CommercialApiUrl { get; private set; } = null!;

    /// <summary>
    ///     API URL for discounted rates (TBC Concept). Null if not supported.
    /// </summary>
    public string? DiscountedApiUrl { get; }

    /// <summary>
    ///     Is bank active for monitoring.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    ///     Display name.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    ///     Factory method to create bank.
    /// </summary>
    public static Bank Create(string code, string name, string commercialApiUrl, string? discountedApiUrl = null)
    {
        return new Bank(BankIdentity.New(), code, name, commercialApiUrl, discountedApiUrl);
    }

    /// <summary>
    ///     Activate bank.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }

    /// <summary>
    ///     Deactivate bank.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    ///     Check if bank supports discounted rates.
    /// </summary>
    public bool SupportsDiscountedRates()
    {
        return !string.IsNullOrEmpty(DiscountedApiUrl);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

using DepositStopLoss.Domain.SharedKernel;

namespace DepositStopLoss.Domain.Banking;

/// <summary>
///     Bank entity. Represents supported banks (TBC, BOG, etc.).
/// </summary>
public sealed class Bank : Entity<BankIdentity>
{
    private readonly List<BankRateSource> _rateSources = new();

    private Bank()
    {
    }

    private Bank(BankIdentity id, string code, string name, BankType type)
        : base(id)
    {
        Code = code;
        Name = name;
        Type = type;
        IsActive = true;
    }

    /// <summary>
    ///     Bank code (TBC, BOG, NBG).
    /// </summary>
    public string Code { get; private set; } = null!;

    /// <summary>
    ///     Is bank active for monitoring.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    ///     Display name.
    /// </summary>
    public string Name { get; private set; } = null!;

    /// <summary>
    ///     Exchange rate sources configured for this bank.
    /// </summary>
    public IReadOnlyList<BankRateSource> RateSources => _rateSources.AsReadOnly();

    /// <summary>
    ///     Bank type (Commercial or CentralBank).
    /// </summary>
    public BankType Type { get; private set; }

    /// <summary>
    ///     Factory method to create bank.
    /// </summary>
    public static Bank Create(string code, string name, BankType type)
    {
        return new Bank(BankIdentity.New(), code, name, type);
    }

    /// <summary>
    ///     Whether this bank accepts deposits.
    /// </summary>
    public bool AcceptsDeposits()
    {
        return Type is BankType.Commercial;
    }

    /// <summary>
    ///     Activate bank.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }

    /// <summary>
    ///     Add a rate source for this bank.
    /// </summary>
    public BankRateSource AddRateSource(RateType rateType, string apiUrl)
    {
        if (_rateSources.Any(rs => rs.RateType == rateType))
        {
            throw new InvalidOperationException($"Rate source for {rateType} already exists");
        }

        var rateSource = BankRateSource.Create(Id, rateType, apiUrl);
        _rateSources.Add(rateSource);

        return rateSource;
    }

    /// <summary>
    ///     Deactivate bank.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    ///     Get API URL for a specific rate type. Returns null if not supported.
    /// </summary>
    public string? GetApiUrl(RateType rateType)
    {
        return _rateSources.FirstOrDefault(rs => rs.RateType == rateType)?.ApiUrl;
    }

    /// <summary>
    ///     Check if bank supports a specific rate type.
    /// </summary>
    public bool SupportsRateType(RateType rateType)
    {
        return _rateSources.Any(rs => rs.RateType == rateType);
    }
}

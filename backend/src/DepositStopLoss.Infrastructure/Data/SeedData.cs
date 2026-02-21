using System;

using DepositStopLoss.Domain.Banking;
using DepositStopLoss.Domain.SharedKernel;

namespace DepositStopLoss.Infrastructure.Data;

/// <summary>
///     Fixed GUIDs for seed data. Using deterministic IDs ensures that
///     EF Core migrations won't generate DELETE+INSERT on every new migration.
/// </summary>
public static class SeedData
{
    // Bank IDs
    public static readonly Guid TbcBankId = new("39364DEE-7CB1-4273-8DFC-6EFA5F5C1C53");
    public static readonly Guid BogBankId = new("AFBB4296-A5F8-428F-A7C6-9EDC9C7B5673");
    public static readonly Guid NbgBankId = new("C73E6BFC-5EFE-41D1-B80A-8CF425E75B3B");

    // BankRateSource IDs
    public static readonly Guid TbcCommercialSourceId = new("CCBA9C58-034A-4B1D-8431-C74249969C0F");
    public static readonly Guid TbcDiscountedSourceId = new("6099CFD8-8A37-4B84-85B3-B0B880394CAC");
    public static readonly Guid BogCommercialSourceId = new("EAAB9B5A-86E2-47DC-ADAD-9330CF6D9263");
    public static readonly Guid NbgCommercialSourceId = new("BA1A049A-7BB2-4213-835F-56B20AA13DFC");

    /// <summary>
    ///     Seed data for banks table.
    ///     HasData() expects CLR property types (BankIdentity, BankType), not raw DB types.
    /// </summary>
    public static object[] GetBanks()
    {
        return
        [
            new { Id = new BankIdentity(TbcBankId), Code = "TBC", Name = "TBC Bank", Type = BankType.Commercial, IsActive = true },
            new { Id = new BankIdentity(BogBankId), Code = "BOG", Name = "Bank of Georgia", Type = BankType.Commercial, IsActive = true },
            new { Id = new BankIdentity(NbgBankId), Code = "NBG", Name = "National Bank of Georgia", Type = BankType.CentralBank, IsActive = true },
        ];
    }

    /// <summary>
    ///     Seed data for bank_rate_sources table.
    /// </summary>
    public static object[] GetBankRateSources()
    {
        return
        [
            new { Id = new BankRateSourceIdentity(TbcCommercialSourceId), BankId = new BankIdentity(TbcBankId), RateType = RateType.Commercial, ApiUrl = "https://api.tbcbank.ge/v1/exchange-rates/commercial" },
            new { Id = new BankRateSourceIdentity(TbcDiscountedSourceId), BankId = new BankIdentity(TbcBankId), RateType = RateType.Discounted, ApiUrl = "https://api.tbcbank.ge/v1/exchange-rates/concept" },
            new { Id = new BankRateSourceIdentity(BogCommercialSourceId), BankId = new BankIdentity(BogBankId), RateType = RateType.Commercial, ApiUrl = "https://bankofgeorgia.ge/api/currencies" },
            new { Id = new BankRateSourceIdentity(NbgCommercialSourceId), BankId = new BankIdentity(NbgBankId), RateType = RateType.Commercial, ApiUrl = "https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/en/json" },
        ];
    }
}

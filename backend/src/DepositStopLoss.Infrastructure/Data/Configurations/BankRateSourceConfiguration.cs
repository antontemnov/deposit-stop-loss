using DepositStopLoss.Domain.Banking;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositStopLoss.Infrastructure.Data.Configurations;

/// <summary>
///     EF Core configuration for BankRateSource entity.
/// </summary>
public sealed class BankRateSourceConfiguration : IEntityTypeConfiguration<BankRateSource>
{
    public void Configure(EntityTypeBuilder<BankRateSource> builder)
    {
        builder.ToTable("bank_rate_sources");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasConversion(id => id.Value, value => new BankRateSourceIdentity(value));

        builder
            .Property(x => x.BankId)
            .HasColumnName("bank_id")
            .HasConversion(id => id.Value, value => new BankIdentity(value))
            .IsRequired();

        builder.Property(x => x.RateType).HasColumnName("rate_type").HasConversion<int>().IsRequired();

        builder.Property(x => x.ApiUrl).HasColumnName("api_url").HasMaxLength(500).IsRequired();

        // Unique: one rate type per bank
        builder
            .HasIndex(x => new { x.BankId, x.RateType })
            .IsUnique()
            .HasDatabaseName("ix_bank_rate_sources_bank_rate_type");

        // Seed data — applied in a separate migration (SeedBanks)
        builder.HasData(SeedData.GetBankRateSources());
    }
}

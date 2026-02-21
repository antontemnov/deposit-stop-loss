using DepositStopLoss.Domain.Banking;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositStopLoss.Infrastructure.Data.Configurations;

/// <summary>
///     EF Core configuration for Bank entity.
/// </summary>
public sealed class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.ToTable("banks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasConversion(id => id.Value, value => new BankIdentity(value));

        builder.Property(x => x.Code).HasColumnName("code").HasMaxLength(10).IsRequired();

        builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(100).IsRequired();

        builder.Property(x => x.Type).HasColumnName("type").HasConversion<int>().IsRequired();

        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();

        // One-to-many: Bank → BankRateSource
        builder.HasMany(x => x.RateSources).WithOne().HasForeignKey(x => x.BankId).OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => x.Code).IsUnique().HasDatabaseName("ix_banks_code");

        // Seed data — applied in a separate migration (SeedBanks)
        builder.HasData(SeedData.GetBanks());
    }
}

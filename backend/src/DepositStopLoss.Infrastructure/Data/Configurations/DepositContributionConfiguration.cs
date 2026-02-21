using DepositStopLoss.Domain.Deposits;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositStopLoss.Infrastructure.Data.Configurations;

/// <summary>
///     EF Core configuration for DepositContribution entity.
/// </summary>
public sealed class DepositContributionConfiguration : IEntityTypeConfiguration<DepositContribution>
{
    public void Configure(EntityTypeBuilder<DepositContribution> builder)
    {
        builder.ToTable("deposit_contributions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasConversion(id => id.Value, value => new DepositContributionIdentity(value));

        builder
            .Property(x => x.DepositId)
            .HasColumnName("deposit_id")
            .HasConversion(id => id.Value, value => new DepositIdentity(value))
            .IsRequired();

        builder.Property(x => x.Amount).HasColumnName("amount").HasPrecision(18, 2).IsRequired();

        builder.Property(x => x.ExchangeRateAtContribution).HasColumnName("exchange_rate").HasPrecision(18, 6).IsRequired();

        // ValueInUsd is derived (Amount / ExchangeRate) — not mapped
        builder.Ignore(x => x.ValueInUsd);

        builder.Property(x => x.ContributedAt).HasColumnName("contributed_at").IsRequired();

        // Indexes
        builder.HasIndex(x => x.DepositId).HasDatabaseName("ix_deposit_contributions_deposit_id");

        // Ignore domain events
        builder.Ignore(x => x.DomainEvents);
    }
}

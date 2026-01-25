using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.Snapshots;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositStopLoss.Infrastructure.Data.Configurations;

/// <summary>
///     EF Core configuration for DepositSnapshot entity.
///     Note: This is a stub configuration.
/// </summary>
public sealed class DepositSnapshotConfiguration : IEntityTypeConfiguration<DepositSnapshot>
{
    public void Configure(EntityTypeBuilder<DepositSnapshot> builder)
    {
        builder.ToTable("deposit_snapshots");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasConversion(id => id.Value, value => new DepositSnapshotIdentity(value));

        builder
            .Property(x => x.DepositId)
            .HasColumnName("deposit_id")
            .HasConversion(id => id.Value, value => new DepositIdentity(value))
            .IsRequired();

        // TODO: Configure Money and Percentage value objects properly
        builder.Ignore(x => x.TotalAmountInDepositCurrency);
        builder.Ignore(x => x.TotalAmountInUsd);
        builder.Ignore(x => x.AccruedInterest);
        builder.Ignore(x => x.ProfitLossPercent);

        builder.Property(x => x.ExchangeRate).HasColumnName("exchange_rate").HasPrecision(18, 6).IsRequired();

        builder.Property(x => x.SnapshotDate).HasColumnName("snapshot_date").IsRequired();

        // Indexes
        builder.HasIndex(x => x.DepositId).HasDatabaseName("ix_deposit_snapshots_deposit_id");

        builder
            .HasIndex(x => new
            {
                x.DepositId,
                x.SnapshotDate,
            })
            .HasDatabaseName("ix_deposit_snapshots_deposit_date");
    }
}

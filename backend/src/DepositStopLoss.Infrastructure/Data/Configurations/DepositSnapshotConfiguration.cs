using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.SharedKernel;
using DepositStopLoss.Domain.Snapshots;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositStopLoss.Infrastructure.Data.Configurations;

/// <summary>
///     EF Core configuration for DepositSnapshot entity.
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

        // Money TotalAmountInDepositCurrency → ComplexProperty
        builder.ComplexProperty(
            x => x.TotalAmountInDepositCurrency,
            money =>
            {
                money.Property(m => m.Amount).HasColumnName("total_amount_deposit_currency").HasPrecision(18, 2).IsRequired();
                money.Property(m => m.Currency).HasColumnName("total_amount_deposit_currency_code").HasConversion<int>().IsRequired();
            });

        // Money TotalAmountInUsd → ComplexProperty
        builder.ComplexProperty(
            x => x.TotalAmountInUsd,
            money =>
            {
                money.Property(m => m.Amount).HasColumnName("total_amount_usd").HasPrecision(18, 2).IsRequired();
                money.Property(m => m.Currency).HasColumnName("total_amount_usd_currency").HasConversion<int>().IsRequired();
            });

        // Money AccruedInterest → ComplexProperty
        builder.ComplexProperty(
            x => x.AccruedInterest,
            money =>
            {
                money.Property(m => m.Amount).HasColumnName("accrued_interest").HasPrecision(18, 2).IsRequired();
                money.Property(m => m.Currency).HasColumnName("accrued_interest_currency").HasConversion<int>().IsRequired();
            });

        // Percentage ProfitLossPercent → ValueConverter
        builder
            .Property(x => x.ProfitLossPercent)
            .HasColumnName("profit_loss_percent")
            .HasPrecision(8, 2)
            .HasConversion(p => p.Value, v => Percentage.FromValue(v))
            .IsRequired();

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

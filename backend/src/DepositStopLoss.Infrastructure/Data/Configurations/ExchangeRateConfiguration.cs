using DepositStopLoss.Domain.Banking;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositStopLoss.Infrastructure.Data.Configurations;

/// <summary>
///     EF Core configuration for ExchangeRate entity.
///     Note: This is a stub configuration.
/// </summary>
public sealed class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
{
    public void Configure(EntityTypeBuilder<ExchangeRate> builder)
    {
        builder.ToTable("exchange_rates");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasConversion(id => id.Value, value => new ExchangeRateIdentity(value));

        builder
            .Property(x => x.BankId)
            .HasColumnName("bank_id")
            .HasConversion(id => id.Value, value => new BankIdentity(value))
            .IsRequired();

        // TODO: Configure Currency value objects properly
        builder.Ignore(x => x.FromCurrency);
        builder.Ignore(x => x.ToCurrency);

        builder.Property(x => x.BuyRate).HasColumnName("buy_rate").HasPrecision(18, 6).IsRequired();

        builder.Property(x => x.SellRate).HasColumnName("sell_rate").HasPrecision(18, 6).IsRequired();

        builder.Property(x => x.RateDate).HasColumnName("rate_date").IsRequired();

        builder.Property(x => x.RateType).HasColumnName("rate_type").HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.Property(x => x.FetchedAt).HasColumnName("fetched_at").IsRequired();

        // Indexes
        builder.HasIndex(x => x.RateDate).HasDatabaseName("ix_exchange_rates_rate_date");
    }
}

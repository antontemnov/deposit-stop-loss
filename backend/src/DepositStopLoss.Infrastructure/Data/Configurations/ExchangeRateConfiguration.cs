using DepositStopLoss.Domain.Banking;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositStopLoss.Infrastructure.Data.Configurations;

/// <summary>
///     EF Core configuration for ExchangeRate entity.
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

        // Currency enums → integer storage
        builder.Property(x => x.FromCurrency).HasColumnName("from_currency").HasConversion<int>().IsRequired();

        builder.Property(x => x.ToCurrency).HasColumnName("to_currency").HasConversion<int>().IsRequired();

        builder.Property(x => x.BuyRate).HasColumnName("buy_rate").HasPrecision(18, 6).IsRequired();

        builder.Property(x => x.SellRate).HasColumnName("sell_rate").HasPrecision(18, 6).IsRequired();

        builder.Property(x => x.RateDate).HasColumnName("rate_date").IsRequired();

        builder.Property(x => x.RateType).HasColumnName("rate_type").HasConversion<int>().IsRequired();

        builder.Property(x => x.FetchedAt).HasColumnName("fetched_at").IsRequired();

        // Indexes
        builder.HasIndex(x => x.RateDate).HasDatabaseName("ix_exchange_rates_rate_date");

        // Unique composite index for rate lookup
        builder
            .HasIndex(x => new
            {
                x.BankId,
                x.FromCurrency,
                x.ToCurrency,
                x.RateDate,
                x.RateType,
            })
            .IsUnique()
            .HasDatabaseName("ix_exchange_rates_bank_currencies_date_type");
    }
}

using DepositStopLoss.Domain.Banking;
using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.SharedKernel;
using DepositStopLoss.Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositStopLoss.Infrastructure.Data.Configurations;

/// <summary>
///     EF Core configuration for Deposit aggregate.
/// </summary>
public sealed class DepositConfiguration : IEntityTypeConfiguration<Deposit>
{
    public void Configure(EntityTypeBuilder<Deposit> builder)
    {
        builder.ToTable("deposits");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasConversion(id => id.Value, value => new DepositIdentity(value));

        builder
            .Property(x => x.UserId)
            .HasColumnName("user_id")
            .HasConversion(id => id.Value, value => new UserIdentity(value))
            .IsRequired();

        builder
            .Property(x => x.BankId)
            .HasColumnName("bank_id")
            .HasConversion(id => id.Value, value => new BankIdentity(value))
            .IsRequired();

        builder.Property(x => x.Currency).HasColumnName("currency").HasConversion<int>().IsRequired();

        // Percentage → ValueConverter (stores decimal value directly)
        builder
            .Property(x => x.AnnualInterestRate)
            .HasColumnName("annual_interest_rate")
            .HasPrecision(5, 2)
            .HasConversion(p => p.Value, v => Percentage.FromValue(v))
            .IsRequired();

        // Percentage → ValueConverter
        builder
            .Property(x => x.StopLossThreshold)
            .HasColumnName("stop_loss_threshold")
            .HasPrecision(5, 2)
            .HasConversion(p => p.Value, v => Percentage.FromValue(v))
            .IsRequired();

        builder.Property(x => x.TermMonths).HasColumnName("term_months").IsRequired();

        builder.Property(x => x.OpenedAt).HasColumnName("opened_at").IsRequired();

        builder.Property(x => x.MaturityDate).HasColumnName("maturity_date").IsRequired();

        builder.Property(x => x.ClosedAt).HasColumnName("closed_at");

        // Enums → integer storage
        builder.Property(x => x.Status).HasColumnName("status").HasConversion<int>().IsRequired();

        builder.Property(x => x.Source).HasColumnName("source").HasConversion<int>().IsRequired();

        builder.Property(x => x.RateType).HasColumnName("rate_type").HasConversion<int>().IsRequired();

        // CurrentAmount is derived from contributions — not mapped
        builder.Ignore(x => x.CurrentAmount);

        // HasMany relationship to DepositContribution
        builder.HasMany(x => x.Contributions).WithOne().HasForeignKey(x => x.DepositId).OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(x => x.UserId).HasDatabaseName("ix_deposits_user_id");

        builder.HasIndex(x => x.BankId).HasDatabaseName("ix_deposits_bank_id");

        builder.HasIndex(x => x.Status).HasDatabaseName("ix_deposits_status");

        // Ignore domain events (not persisted)
        builder.Ignore(x => x.DomainEvents);
    }
}

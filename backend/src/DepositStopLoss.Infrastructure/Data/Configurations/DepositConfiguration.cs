using DepositStopLoss.Domain.Banking;
using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositStopLoss.Infrastructure.Data.Configurations;

/// <summary>
///     EF Core configuration for Deposit entity.
///     Note: This is a stub configuration. Full implementation requires proper value object mapping.
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

        builder.Property(x => x.TermMonths).HasColumnName("term_months").IsRequired();

        builder.Property(x => x.OpenedAt).HasColumnName("opened_at").IsRequired();

        builder.Property(x => x.MaturityDate).HasColumnName("maturity_date").IsRequired();

        builder.Property(x => x.ClosedAt).HasColumnName("closed_at");

        builder.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.Property(x => x.Source).HasColumnName("source").HasConversion<string>().HasMaxLength(20).IsRequired();

        builder.Property(x => x.RateType).HasColumnName("rate_type").HasConversion<string>().HasMaxLength(20).IsRequired();

        // TODO: Configure Money and Percentage value objects properly
        // For now, ignore complex navigation properties
        builder.Ignore(x => x.InitialAmount);
        builder.Ignore(x => x.CurrentAmount);
        builder.Ignore(x => x.AnnualInterestRate);
        builder.Ignore(x => x.StopLossThreshold);
        builder.Ignore(x => x.Contributions);

        // Indexes
        builder.HasIndex(x => x.UserId).HasDatabaseName("ix_deposits_user_id");

        builder.HasIndex(x => x.BankId).HasDatabaseName("ix_deposits_bank_id");

        builder.HasIndex(x => x.Status).HasDatabaseName("ix_deposits_status");

        // Ignore domain events (not persisted)
        builder.Ignore(x => x.DomainEvents);
    }
}

using DepositStopLoss.Domain.Banking;
using DepositStopLoss.Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositStopLoss.Infrastructure.Data.Configurations;

/// <summary>
///     EF Core configuration for UserBankConnection entity.
/// </summary>
public sealed class UserBankConnectionConfiguration : IEntityTypeConfiguration<UserBankConnection>
{
    public void Configure(EntityTypeBuilder<UserBankConnection> builder)
    {
        builder.ToTable("user_bank_connections");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasConversion(id => id.Value, value => new UserBankConnectionIdentity(value));

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

        builder.Property(x => x.AccessToken).HasColumnName("access_token").HasMaxLength(2000);

        builder.Property(x => x.RefreshToken).HasColumnName("refresh_token").HasMaxLength(2000);

        builder.Property(x => x.TokenExpiresAt).HasColumnName("token_expires_at");

        // PostgreSQL text[] for scopes
        builder.Property(x => x.Scopes).HasColumnName("scopes").HasColumnType("text[]");

        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();

        builder.Property(x => x.ConnectedAt).HasColumnName("connected_at").IsRequired();

        builder.Property(x => x.LastSyncAt).HasColumnName("last_sync_at");

        // Indexes
        builder
            .HasIndex(x => new
            {
                x.UserId,
                x.BankId,
            })
            .IsUnique()
            .HasDatabaseName("ix_user_bank_connections_user_bank");
    }
}

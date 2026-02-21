using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.Notifications;
using DepositStopLoss.Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositStopLoss.Infrastructure.Data.Configurations;

/// <summary>
///     EF Core configuration for Notification entity.
/// </summary>
public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("notifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasConversion(id => id.Value, value => new NotificationIdentity(value));

        builder
            .Property(x => x.UserId)
            .HasColumnName("user_id")
            .HasConversion(id => id.Value, value => new UserIdentity(value))
            .IsRequired();

        builder
            .Property(x => x.DepositId)
            .HasColumnName("deposit_id")
            .HasConversion(id => id.Value, value => new DepositIdentity(value))
            .IsRequired();

        builder.Property(x => x.Level).HasColumnName("level").HasConversion<int>().IsRequired();

        builder.Property(x => x.Message).HasColumnName("message").HasMaxLength(1000).IsRequired();

        builder.Property(x => x.Status).HasColumnName("status").HasConversion<int>().IsRequired();

        builder.Property(x => x.ErrorMessage).HasColumnName("error_message").HasMaxLength(500);

        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired();

        builder.Property(x => x.SentAt).HasColumnName("sent_at");

        builder.Property(x => x.ReadAt).HasColumnName("read_at");

        // Indexes
        builder.HasIndex(x => x.UserId).HasDatabaseName("ix_notifications_user_id");

        builder.HasIndex(x => x.Status).HasDatabaseName("ix_notifications_status");

        builder
            .HasIndex(x => new
            {
                x.UserId,
                x.Status,
            })
            .HasDatabaseName("ix_notifications_user_status");
    }
}

using DepositStopLoss.Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DepositStopLoss.Infrastructure.Data.Configurations;

/// <summary>
///     EF Core configuration for User entity.
/// </summary>
public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasColumnName("id").HasConversion(id => id.Value, value => new UserIdentity(value));

        builder.Property(x => x.TelegramId).HasColumnName("telegram_id").IsRequired();

        builder.Property(x => x.Username).HasColumnName("username").HasMaxLength(100);

        builder.Property(x => x.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();

        builder.Property(x => x.LastName).HasColumnName("last_name").HasMaxLength(100);

        builder.Property(x => x.LanguageCode).HasColumnName("language_code").HasMaxLength(10).IsRequired();

        builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(200);

        builder.Property(x => x.Timezone).HasColumnName("timezone").HasMaxLength(50).IsRequired();

        builder.Property(x => x.EnableTelegramNotifications).HasColumnName("enable_telegram_notifications").IsRequired();

        builder.Property(x => x.EnableEmailNotifications).HasColumnName("enable_email_notifications").IsRequired();

        builder.Property(x => x.IsActive).HasColumnName("is_active").IsRequired();

        builder.Property(x => x.RegisteredAt).HasColumnName("registered_at").IsRequired();

        builder.Property(x => x.LastActivityAt).HasColumnName("last_activity_at");

        // Indexes
        builder.HasIndex(x => x.TelegramId).IsUnique().HasDatabaseName("ix_users_telegram_id");

        // Ignore domain events
        builder.Ignore(x => x.DomainEvents);
    }
}

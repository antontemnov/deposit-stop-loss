using DepositStopLoss.Domain.Banking;
using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.Notifications;
using DepositStopLoss.Domain.Snapshots;
using DepositStopLoss.Domain.Users;

using Microsoft.EntityFrameworkCore;

namespace DepositStopLoss.Infrastructure.Data;

/// <summary>
///     Application database context for PostgreSQL.
/// </summary>
public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Bank> Banks => Set<Bank>();

    public DbSet<Deposit> Deposits => Set<Deposit>();

    public DbSet<DepositSnapshot> DepositSnapshots => Set<DepositSnapshot>();

    public DbSet<ExchangeRate> ExchangeRates => Set<ExchangeRate>();

    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<UserBankConnection> UserBankConnections => Set<UserBankConnection>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Set default schema
        modelBuilder.HasDefaultSchema("public");
    }
}

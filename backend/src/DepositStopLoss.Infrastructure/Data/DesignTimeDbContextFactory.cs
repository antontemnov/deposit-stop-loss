using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DepositStopLoss.Infrastructure.Data;

/// <summary>
///     Factory for creating DbContext at design time (migrations, scaffolding).
///     Used by dotnet-ef tool when no running application is available.
/// </summary>
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Design-time connection string — only used for migration generation
        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=deposit_stop_loss;Username=postgres;Password=postgres",
            npgsql => npgsql.UseNodaTime());

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

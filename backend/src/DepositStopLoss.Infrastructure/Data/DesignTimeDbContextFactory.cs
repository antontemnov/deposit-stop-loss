using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DepositStopLoss.Infrastructure.Data;

/// <summary>
///     Factory for creating DbContext at design time (migrations, scaffolding).
///     Reads connection string from environments.yaml copied to output directory.
/// </summary>
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddYamlFile("settings.yaml", optional: false, reloadOnChange: true)
            .AddYamlFile("environments.yaml", optional: false, reloadOnChange: true)
            .AddYamlFile("secrets.yaml", optional: false, reloadOnChange: true)
            .Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "ConnectionStrings:DefaultConnection not found in environments.yaml");

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsql => npgsql.UseNodaTime());

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}

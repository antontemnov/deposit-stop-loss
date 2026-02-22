using System.Net.Http;

using DepositStopLoss.Application.ExternalServices;
using DepositStopLoss.Application.Persistence;
using DepositStopLoss.Infrastructure.BackgroundJobs;
using DepositStopLoss.Infrastructure.Data;
using DepositStopLoss.Infrastructure.Data.Repositories;
using DepositStopLoss.Infrastructure.ExternalServices;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;

namespace DepositStopLoss.Infrastructure;

/// <summary>
///     Infrastructure layer dependency injection configuration.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(
            configuration.GetConnectionString("DefaultConnection"),
            npgsqlOptions =>
            {
                npgsqlOptions.UseNodaTime();
                npgsqlOptions.EnableRetryOnFailure();
            }));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDepositRepository, DepositRepository>();
        services.AddScoped<IBankRepository, BankRepository>();
        services.AddScoped<IExchangeRateRepository, ExchangeRateRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IDepositSnapshotRepository, DepositSnapshotRepository>();

        // External Services (HttpClient registration requires Microsoft.Extensions.Http package)
        // TODO: Add HttpClient factory registration
        // services.AddHttpClient<NbgExchangeRateProvider>();
        // services.AddHttpClient<TbcExchangeRateProvider>();
        // services.AddHttpClient<BogExchangeRateProvider>();

        // For now, register providers without HttpClient factory
        services.AddScoped<IExchangeRateProvider, NbgExchangeRateProvider>(sp => new NbgExchangeRateProvider(new HttpClient()));

        services.AddScoped<IExchangeRateProvider, TbcExchangeRateProvider>(sp => new TbcExchangeRateProvider(new HttpClient()));

        services.AddScoped<IExchangeRateProvider, BogExchangeRateProvider>(sp => new BogExchangeRateProvider(new HttpClient()));

        // Quartz Background Jobs
        services.AddQuartz(q =>
        {
            // Exchange Rate Fetch Job - every 10 minutes
            q.AddJob<ExchangeRateFetchJob>(opts => opts.WithIdentity("ExchangeRateFetchJob"));

            q.AddTrigger(opts =>
                opts.ForJob("ExchangeRateFetchJob").WithIdentity("ExchangeRateFetchJob-trigger").WithCronSchedule("0 */10 * * * ?"));

            // Deposit Monitor Job - every 5 minutes
            q.AddJob<DepositMonitorJob>(opts => opts.WithIdentity("DepositMonitorJob"));

            q.AddTrigger(opts =>
                opts.ForJob("DepositMonitorJob").WithIdentity("DepositMonitorJob-trigger").WithCronSchedule("0 */5 * * * ?"));

            // Notification Sender Job - every 2 minutes
            q.AddJob<NotificationSenderJob>(opts => opts.WithIdentity("NotificationSenderJob"));

            q.AddTrigger(opts =>
                opts.ForJob("NotificationSenderJob").WithIdentity("NotificationSenderJob-trigger").WithCronSchedule("0 */2 * * * ?"));

            // Cleanup Job - daily at 2:00 AM
            q.AddJob<CleanupJob>(opts => opts.WithIdentity("CleanupJob"));
            q.AddTrigger(opts => opts.ForJob("CleanupJob").WithIdentity("CleanupJob-trigger").WithCronSchedule("0 0 2 * * ?"));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        return services;
    }
}

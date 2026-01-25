using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Quartz;

namespace DepositStopLoss.Infrastructure.BackgroundJobs;

/// <summary>
///     Background job to fetch exchange rates from external APIs.
///     Runs every 10 minutes to update rates in database.
/// </summary>
[DisallowConcurrentExecution]
public sealed partial class ExchangeRateFetchJob : IJob
{
    private readonly ILogger<ExchangeRateFetchJob> _logger;

    public ExchangeRateFetchJob(ILogger<ExchangeRateFetchJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        LogJobStarted();

        // TODO: Implement
        // 1. Get all active banks from repository
        // 2. For each bank, fetch current rates using IExchangeRateProvider
        // 3. Save new rates to database (if different from last saved)
        // 4. Log results
        LogJobCompleted();

        return Task.CompletedTask;
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "ExchangeRateFetchJob started")]
    private partial void LogJobStarted();

    [LoggerMessage(Level = LogLevel.Information, Message = "ExchangeRateFetchJob completed")]
    private partial void LogJobCompleted();
}

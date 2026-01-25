using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Quartz;

namespace DepositStopLoss.Infrastructure.BackgroundJobs;

/// <summary>
///     Background job for cleanup tasks.
///     Runs daily at 2:00 AM.
/// </summary>
[DisallowConcurrentExecution]
public sealed partial class CleanupJob : IJob
{
    private readonly ILogger<CleanupJob> _logger;

    public CleanupJob(ILogger<CleanupJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        LogJobStarted();

        // TODO: Implement
        // 1. Delete old notifications (older than 30 days)
        // 2. Delete old snapshots (keep last N per deposit)
        // 3. Delete old exchange rates (keep last 2 years)
        LogJobCompleted();

        return Task.CompletedTask;
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "CleanupJob started")]
    private partial void LogJobStarted();

    [LoggerMessage(Level = LogLevel.Information, Message = "CleanupJob completed")]
    private partial void LogJobCompleted();
}

using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Quartz;

namespace DepositStopLoss.Infrastructure.BackgroundJobs;

/// <summary>
///     Background job to monitor deposits and check profitability.
///     Creates snapshots and sends notifications when stop-loss threshold is approached.
/// </summary>
[DisallowConcurrentExecution]
public sealed partial class DepositMonitorJob : IJob
{
    private readonly ILogger<DepositMonitorJob> _logger;

    public DepositMonitorJob(ILogger<DepositMonitorJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        LogJobStarted();

        // TODO: Implement
        // 1. Get all active deposits
        // 2. For each deposit:
        //    a. Get current exchange rate
        //    b. Calculate current profitability using DepositCalculator
        //    c. Create DepositSnapshot
        //    d. Check if near stop-loss threshold
        //    e. If threshold reached, create Notification
        // 3. Save all changes
        LogJobCompleted();

        return Task.CompletedTask;
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "DepositMonitorJob started")]
    private partial void LogJobStarted();

    [LoggerMessage(Level = LogLevel.Information, Message = "DepositMonitorJob completed")]
    private partial void LogJobCompleted();
}

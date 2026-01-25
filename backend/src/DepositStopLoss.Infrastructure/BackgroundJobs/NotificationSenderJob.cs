using System.Threading.Tasks;

using Microsoft.Extensions.Logging;
using Quartz;

namespace DepositStopLoss.Infrastructure.BackgroundJobs;

/// <summary>
///     Background job to send pending notifications via Telegram.
///     Runs every 2 minutes.
/// </summary>
[DisallowConcurrentExecution]
public sealed partial class NotificationSenderJob : IJob
{
    private readonly ILogger<NotificationSenderJob> _logger;

    public NotificationSenderJob(ILogger<NotificationSenderJob> logger)
    {
        _logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        LogJobStarted();

        // TODO: Implement
        // 1. Get all pending notifications from repository
        // 2. For each notification:
        //    a. Get user's Telegram chat ID
        //    b. Send message via Telegram Bot API
        //    c. Mark notification as Sent or Failed
        // 3. Save changes
        LogJobCompleted();

        return Task.CompletedTask;
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "NotificationSenderJob started")]
    private partial void LogJobStarted();

    [LoggerMessage(Level = LogLevel.Information, Message = "NotificationSenderJob completed")]
    private partial void LogJobCompleted();
}

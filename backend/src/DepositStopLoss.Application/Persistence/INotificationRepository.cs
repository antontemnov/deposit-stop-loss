using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.Notifications;
using DepositStopLoss.Domain.Users;

namespace DepositStopLoss.Application.Persistence;

/// <summary>
///     Repository interface for Notification entity.
/// </summary>
public interface INotificationRepository
{
    Task AddAsync(Notification notification, CancellationToken cancellationToken = default);

    Task DeleteOlderThanAsync(int daysOld, CancellationToken cancellationToken = default);

    Task<Notification?> GetByIdAsync(NotificationIdentity id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Notification>> GetByUserIdAsync(UserIdentity userId, CancellationToken cancellationToken = default);

    Task<Notification?> GetLastByDepositAndLevelAsync(
        DepositIdentity depositId,
        NotificationLevel level,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Notification>> GetPendingAsync(CancellationToken cancellationToken = default);

    Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default);
}

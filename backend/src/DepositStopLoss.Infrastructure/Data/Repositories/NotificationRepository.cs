using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application.Repositories;
using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.Notifications;
using DepositStopLoss.Domain.Users;

namespace DepositStopLoss.Infrastructure.Data.Repositories;

/// <summary>
///     EF Core implementation of INotificationRepository.
/// </summary>
public sealed class NotificationRepository : INotificationRepository
{
    private readonly ApplicationDbContext _context;

    public NotificationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }

    public Task DeleteOlderThanAsync(int daysOld, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }

    public Task<Notification?> GetByIdAsync(NotificationIdentity id, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }

    public Task<IReadOnlyList<Notification>> GetByUserIdAsync(UserIdentity userId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }

    public Task<Notification?> GetLastByDepositAndLevelAsync(
        DepositIdentity depositId,
        NotificationLevel level,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }

    public Task<IReadOnlyList<Notification>> GetPendingAsync(CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }

    public Task UpdateAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }
}

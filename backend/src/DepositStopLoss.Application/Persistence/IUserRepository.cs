using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Domain.Users;

namespace DepositStopLoss.Application.Persistence;

/// <summary>
///     Repository interface for User aggregate.
/// </summary>
public interface IUserRepository
{
    Task AddAsync(User user, CancellationToken cancellationToken = default);

    Task<bool> ExistsByTelegramIdAsync(long telegramId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<User>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(UserIdentity id, CancellationToken cancellationToken = default);

    Task<User?> GetByTelegramIdAsync(long telegramId, CancellationToken cancellationToken = default);

    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
}

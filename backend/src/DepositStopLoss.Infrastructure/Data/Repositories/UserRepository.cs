using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application.Persistence;
using DepositStopLoss.Domain.Users;

namespace DepositStopLoss.Infrastructure.Data.Repositories;

/// <summary>
///     EF Core implementation of IUserRepository.
/// </summary>
public sealed class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task<bool> ExistsByTelegramIdAsync(long telegramId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task<IReadOnlyList<User>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task<User?> GetByIdAsync(UserIdentity id, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task<User?> GetByTelegramIdAsync(long telegramId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }
}

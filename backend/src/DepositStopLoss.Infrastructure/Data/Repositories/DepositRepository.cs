using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application.Persistence;
using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.Users;

namespace DepositStopLoss.Infrastructure.Data.Repositories;

/// <summary>
///     EF Core implementation of IDepositRepository.
/// </summary>
public sealed class DepositRepository : IDepositRepository
{
    private readonly ApplicationDbContext _context;

    public DepositRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Deposit deposit, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task DeleteAsync(Deposit deposit, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task<IReadOnlyList<Deposit>> GetActiveDepositsAsync(CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task<Deposit?> GetByIdAsync(DepositIdentity id, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task<IReadOnlyList<Deposit>> GetByUserIdAsync(UserIdentity userId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task UpdateAsync(Deposit deposit, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }
}

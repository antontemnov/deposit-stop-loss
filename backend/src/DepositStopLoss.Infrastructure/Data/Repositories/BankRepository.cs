using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application.Repositories;
using DepositStopLoss.Domain.Banking;

namespace DepositStopLoss.Infrastructure.Data.Repositories;

/// <summary>
///     EF Core implementation of IBankRepository.
/// </summary>
public sealed class BankRepository : IBankRepository
{
    private readonly ApplicationDbContext _context;

    public BankRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(Bank bank, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }

    public Task<IReadOnlyList<Bank>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }

    public Task<Bank?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }

    public Task<Bank?> GetByIdAsync(BankIdentity id, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }

    public Task UpdateAsync(Bank bank, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Domain.Banking;

namespace DepositStopLoss.Application.Repositories;

/// <summary>
///     Repository interface for Bank entity.
/// </summary>
public interface IBankRepository
{
    Task AddAsync(Bank bank, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Bank>> GetAllActiveAsync(CancellationToken cancellationToken = default);

    Task<Bank?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<Bank?> GetByIdAsync(BankIdentity id, CancellationToken cancellationToken = default);

    Task UpdateAsync(Bank bank, CancellationToken cancellationToken = default);
}

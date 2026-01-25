using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.Users;

namespace DepositStopLoss.Application.Repositories;

/// <summary>
///     Repository interface for Deposit aggregate.
/// </summary>
public interface IDepositRepository
{
    Task AddAsync(Deposit deposit, CancellationToken cancellationToken = default);

    Task DeleteAsync(Deposit deposit, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Deposit>> GetActiveDepositsAsync(CancellationToken cancellationToken = default);

    Task<Deposit?> GetByIdAsync(DepositIdentity id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Deposit>> GetByUserIdAsync(UserIdentity userId, CancellationToken cancellationToken = default);

    Task UpdateAsync(Deposit deposit, CancellationToken cancellationToken = default);
}

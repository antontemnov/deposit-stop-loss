using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.Snapshots;

using NodaTime;

namespace DepositStopLoss.Application.Persistence;

/// <summary>
///     Repository interface for DepositSnapshot entity.
/// </summary>
public interface IDepositSnapshotRepository
{
    Task AddAsync(DepositSnapshot snapshot, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<DepositSnapshot> snapshots, CancellationToken cancellationToken = default);

    Task DeleteOldestForDepositAsync(DepositIdentity depositId, int keepCount, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DepositSnapshot>> GetByDepositIdAsync(DepositIdentity depositId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<DepositSnapshot>> GetByDepositInRangeAsync(
        DepositIdentity depositId,
        Instant startDate,
        Instant endDate,
        CancellationToken cancellationToken = default);

    Task<DepositSnapshot?> GetLatestForDepositAsync(DepositIdentity depositId, CancellationToken cancellationToken = default);
}

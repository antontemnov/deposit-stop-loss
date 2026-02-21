using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application.Persistence;
using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.Snapshots;

using NodaTime;

namespace DepositStopLoss.Infrastructure.Data.Repositories;

/// <summary>
///     EF Core implementation of IDepositSnapshotRepository.
/// </summary>
public sealed class DepositSnapshotRepository : IDepositSnapshotRepository
{
    private readonly ApplicationDbContext _context;

    public DepositSnapshotRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(DepositSnapshot snapshot, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task AddRangeAsync(IEnumerable<DepositSnapshot> snapshots, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task DeleteOldestForDepositAsync(DepositIdentity depositId, int keepCount, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task<IReadOnlyList<DepositSnapshot>> GetByDepositIdAsync(
        DepositIdentity depositId,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task<IReadOnlyList<DepositSnapshot>> GetByDepositInRangeAsync(
        DepositIdentity depositId,
        Instant startDate,
        Instant endDate,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task<DepositSnapshot?> GetLatestForDepositAsync(DepositIdentity depositId, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }
}

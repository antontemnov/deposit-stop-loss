using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application.Persistence;

namespace DepositStopLoss.Infrastructure.Data;

/// <summary>
///     EF Core implementation of Unit of Work.
/// </summary>
public sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return dbContext.SaveChangesAsync(ct);
    }
}

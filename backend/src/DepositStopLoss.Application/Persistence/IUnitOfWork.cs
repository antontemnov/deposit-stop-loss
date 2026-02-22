using System.Threading;
using System.Threading.Tasks;

namespace DepositStopLoss.Application.Persistence;

/// <summary>
///     Unit of Work abstraction for coordinating persistence.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

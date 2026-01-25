using System.Threading;
using System.Threading.Tasks;

namespace DepositStopLoss.Application;

/// <summary>
///     Handler interface for CQRS queries.
/// </summary>
/// <typeparam name="TQuery">Query type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    Task<TResponse> Handle(TQuery query, CancellationToken cancellationToken);
}

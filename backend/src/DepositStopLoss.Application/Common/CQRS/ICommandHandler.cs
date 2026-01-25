using System.Threading;
using System.Threading.Tasks;

namespace DepositStopLoss.Application;

/// <summary>
///     Handler interface for CQRS commands.
/// </summary>
/// <typeparam name="TCommand">Command type.</typeparam>
/// <typeparam name="TResponse">Response type.</typeparam>
public interface ICommandHandler<in TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
}

using System;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application;

using ErrorOr;

namespace DepositStopLoss.Application.Features.Deposits.Commands.CloseDeposit;

/// <summary>
///     Handler for CloseDepositCommand.
/// </summary>
public sealed class CloseDepositCommandHandler : ICommandHandler<CloseDepositCommand, ErrorOr<Success>>
{
    public Task<ErrorOr<Success>> Handle(CloseDepositCommand command, CancellationToken cancellationToken)
    {
        // Stub: Find deposit by Id, call deposit.Close(), save changes, publish DepositClosedEvent
        throw new NotSupportedException("CloseDepositCommandHandler not implemented");
    }
}

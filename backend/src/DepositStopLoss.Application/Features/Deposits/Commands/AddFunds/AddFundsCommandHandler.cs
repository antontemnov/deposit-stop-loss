using System;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application;

using ErrorOr;

namespace DepositStopLoss.Application.Features.Deposits.Commands.AddFunds;

/// <summary>
///     Handler for AddFundsCommand.
/// </summary>
public sealed class AddFundsCommandHandler : ICommandHandler<AddFundsCommand, ErrorOr<Success>>
{
    public Task<ErrorOr<Success>> Handle(AddFundsCommand command, CancellationToken cancellationToken)
    {
        // Stub: Find deposit by Id, get current exchange rate, call deposit.AddFunds(), save changes, publish DepositFundsAddedEvent
        throw new NotSupportedException("AddFundsCommandHandler not implemented");
    }
}

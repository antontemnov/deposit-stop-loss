using System;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

namespace DepositStopLoss.Application.Features.Deposits.Commands.CreateDeposit;

/// <summary>
///     Handler for CreateDepositCommand.
/// </summary>
public sealed class CreateDepositCommandHandler : ICommandHandler<CreateDepositCommand, ErrorOr<DepositDto>>
{
    public Task<ErrorOr<DepositDto>> Handle(CreateDepositCommand command, CancellationToken cancellationToken)
    {
        // Stub: Validate user exists, validate bank exists, create Deposit aggregate, save to repository, return mapped DTO
        throw new NotSupportedException("CreateDepositCommandHandler not implemented");
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

namespace DepositStopLoss.Application.Features.Deposits.Queries.GetDeposit;

/// <summary>
///     Handler for GetDepositQuery.
/// </summary>
public sealed class GetDepositQueryHandler : IQueryHandler<GetDepositQuery, ErrorOr<DepositDto>>
{
    public Task<ErrorOr<DepositDto>> Handle(GetDepositQuery query, CancellationToken cancellationToken)
    {
        // Stub: Find deposit by Id from repository, map to DTO, return
        throw new NotSupportedException("GetDepositQueryHandler not implemented");
    }
}

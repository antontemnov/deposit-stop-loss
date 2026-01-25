using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

namespace DepositStopLoss.Application.Features.Deposits.Queries.GetUserDeposits;

/// <summary>
///     Handler for GetUserDepositsQuery.
/// </summary>
public sealed class GetUserDepositsQueryHandler : IQueryHandler<GetUserDepositsQuery, ErrorOr<IReadOnlyList<DepositDto>>>
{
    public Task<ErrorOr<IReadOnlyList<DepositDto>>> Handle(GetUserDepositsQuery query, CancellationToken cancellationToken)
    {
        // Stub: Get deposits by UserId from repository, map to DTOs, return
        throw new NotSupportedException("GetUserDepositsQueryHandler not implemented");
    }
}

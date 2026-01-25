using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

namespace DepositStopLoss.Application.Features.Banks.Queries.GetBanks;

/// <summary>
///     Handler for GetBanksQuery.
/// </summary>
public sealed class GetBanksQueryHandler : IQueryHandler<GetBanksQuery, ErrorOr<IReadOnlyList<BankDto>>>
{
    public Task<ErrorOr<IReadOnlyList<BankDto>>> Handle(GetBanksQuery query, CancellationToken cancellationToken)
    {
        // Stub: Get all active banks from repository, map to DTOs, return
        throw new NotSupportedException("GetBanksQueryHandler not implemented");
    }
}

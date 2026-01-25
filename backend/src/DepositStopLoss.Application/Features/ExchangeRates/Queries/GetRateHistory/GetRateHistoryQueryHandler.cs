using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

namespace DepositStopLoss.Application.Features.ExchangeRates.Queries.GetRateHistory;

/// <summary>
///     Handler for GetRateHistoryQuery.
/// </summary>
public sealed class GetRateHistoryQueryHandler : IQueryHandler<GetRateHistoryQuery, ErrorOr<IReadOnlyList<ExchangeRateDto>>>
{
    public Task<ErrorOr<IReadOnlyList<ExchangeRateDto>>> Handle(GetRateHistoryQuery query, CancellationToken cancellationToken)
    {
        // Stub: Get rates from DB for the date range, if gaps exist try backfill from external API, return mapped DTOs
        throw new NotSupportedException("GetRateHistoryQueryHandler not implemented");
    }
}

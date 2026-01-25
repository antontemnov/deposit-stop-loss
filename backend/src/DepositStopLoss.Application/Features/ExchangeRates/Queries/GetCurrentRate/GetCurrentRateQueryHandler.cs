using System;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

namespace DepositStopLoss.Application.Features.ExchangeRates.Queries.GetCurrentRate;

/// <summary>
///     Handler for GetCurrentRateQuery.
/// </summary>
public sealed class GetCurrentRateQueryHandler : IQueryHandler<GetCurrentRateQuery, ErrorOr<ExchangeRateDto>>
{
    public Task<ErrorOr<ExchangeRateDto>> Handle(GetCurrentRateQuery query, CancellationToken cancellationToken)
    {
        // Stub: Try to get from cache/DB first, if not found or stale fetch from external API, save to DB, return mapped DTO
        throw new NotSupportedException("GetCurrentRateQueryHandler not implemented");
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Services;

using ErrorOr;

namespace DepositStopLoss.Application.Features.Deposits.Queries.CalculateProfitability;

/// <summary>
///     Handler for CalculateProfitabilityQuery.
/// </summary>
public sealed class CalculateProfitabilityQueryHandler : IQueryHandler<CalculateProfitabilityQuery, ErrorOr<DepositCalculationResult>>
{
    private readonly IDepositCalculator _calculator;

    public CalculateProfitabilityQueryHandler(IDepositCalculator calculator)
    {
        _calculator = calculator;
    }

    public Task<ErrorOr<DepositCalculationResult>> Handle(CalculateProfitabilityQuery query, CancellationToken cancellationToken)
    {
        // Stub: Get deposit from repository, get current exchange rate, use DepositCalculator to calculate profitability, return result
        throw new NotSupportedException("CalculateProfitabilityQueryHandler not implemented");
    }
}

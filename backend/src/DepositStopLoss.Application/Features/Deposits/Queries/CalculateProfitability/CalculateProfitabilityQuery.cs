using System;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Services;

using ErrorOr;

namespace DepositStopLoss.Application.Features.Deposits.Queries.CalculateProfitability;

/// <summary>
///     Query to calculate current profitability of a deposit.
/// </summary>
public sealed record CalculateProfitabilityQuery(Guid DepositId) : IQuery<ErrorOr<DepositCalculationResult>>;

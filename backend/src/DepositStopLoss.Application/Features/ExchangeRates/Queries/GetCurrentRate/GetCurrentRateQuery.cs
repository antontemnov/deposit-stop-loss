using System;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

namespace DepositStopLoss.Application.Features.ExchangeRates.Queries.GetCurrentRate;

/// <summary>
///     Query to get current exchange rate.
/// </summary>
public sealed record GetCurrentRateQuery(Guid BankId, string FromCurrency, string ToCurrency, string RateType)
    : IQuery<ErrorOr<ExchangeRateDto>>;

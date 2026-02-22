using System;

using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

using FastEndpoints;

namespace DepositStopLoss.Application.Features.ExchangeRates.Queries.GetCurrentRate;

/// <summary>
///     Query to get current exchange rate.
/// </summary>
public sealed record GetCurrentRateQuery(Guid BankId, string FromCurrency, string ToCurrency, string RateType)
    : ICommand<ErrorOr<ExchangeRateDto>>;

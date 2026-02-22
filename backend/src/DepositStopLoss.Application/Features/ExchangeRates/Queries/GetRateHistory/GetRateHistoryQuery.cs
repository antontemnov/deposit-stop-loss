using System;
using System.Collections.Generic;

using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

using FastEndpoints;

using NodaTime;

namespace DepositStopLoss.Application.Features.ExchangeRates.Queries.GetRateHistory;

/// <summary>
///     Query to get exchange rate history for a period.
/// </summary>
public sealed record GetRateHistoryQuery(Guid BankId, string FromCurrency, string ToCurrency, string RateType, LocalDate From, LocalDate To)
    : ICommand<ErrorOr<IReadOnlyList<ExchangeRateDto>>>;

using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Application.ExternalServices;

/// <summary>
///     External rate for exchange rate data.
/// </summary>
public sealed record ExternalExchangeRate(
    string FromCurrency,
    string ToCurrency,
    decimal BuyRate,
    decimal SellRate,
    LocalDate RateDate,
    RateType RateType);

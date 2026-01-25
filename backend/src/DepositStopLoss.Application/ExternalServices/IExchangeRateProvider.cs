using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Application.ExternalServices;

/// <summary>
///     Interface for external exchange rate providers (Strategy pattern).
/// </summary>
public interface IExchangeRateProvider
{
    /// <summary>
    ///     Provider code (TBC, BOG, NBG).
    /// </summary>
    string ProviderCode { get; }

    /// <summary>
    ///     Get current exchange rate.
    /// </summary>
    Task<ExternalExchangeRate?> GetCurrentRateAsync(
        string fromCurrency,
        string toCurrency,
        RateType rateType,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get historical exchange rate for specific date.
    /// </summary>
    Task<ExternalExchangeRate?> GetHistoricalRateAsync(
        string fromCurrency,
        string toCurrency,
        LocalDate date,
        RateType rateType,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get historical rates for date range.
    /// </summary>
    Task<IReadOnlyList<ExternalExchangeRate>> GetHistoricalRatesAsync(
        string fromCurrency,
        string toCurrency,
        LocalDate from,
        LocalDate to,
        RateType rateType,
        CancellationToken cancellationToken = default);
}

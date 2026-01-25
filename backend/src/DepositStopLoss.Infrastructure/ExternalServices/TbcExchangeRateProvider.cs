using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application.ExternalServices;
using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Infrastructure.ExternalServices;

/// <summary>
///     TBC Bank exchange rate provider.
///     Supports Commercial and Concept (discounted) rates.
///     https://developers.tbcbank.ge
/// </summary>
public sealed class TbcExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;

    public TbcExchangeRateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string ProviderCode => "TBC";

    public Task<ExternalExchangeRate?> GetCurrentRateAsync(
        string fromCurrency,
        string toCurrency,
        RateType rateType,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement TBC API call
        // Commercial: https://api.tbcbank.ge/v1/exchange-rates/commercial
        // Concept: https://api.tbcbank.ge/v1/exchange-rates/concept
        throw new System.NotSupportedException();
    }

    public Task<ExternalExchangeRate?> GetHistoricalRateAsync(
        string fromCurrency,
        string toCurrency,
        LocalDate date,
        RateType rateType,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }

    public Task<IReadOnlyList<ExternalExchangeRate>> GetHistoricalRatesAsync(
        string fromCurrency,
        string toCurrency,
        LocalDate from,
        LocalDate to,
        RateType rateType,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new System.NotSupportedException();
    }
}

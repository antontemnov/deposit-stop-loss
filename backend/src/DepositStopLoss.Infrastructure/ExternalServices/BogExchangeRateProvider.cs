using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application.ExternalServices;
using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Infrastructure.ExternalServices;

/// <summary>
///     Bank of Georgia exchange rate provider.
///     https://api.bog.ge
/// </summary>
public sealed class BogExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;

    public BogExchangeRateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string ProviderCode => "BOG";

    public Task<ExternalExchangeRate?> GetCurrentRateAsync(
        string fromCurrency,
        string toCurrency,
        RateType rateType,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement BOG API call
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

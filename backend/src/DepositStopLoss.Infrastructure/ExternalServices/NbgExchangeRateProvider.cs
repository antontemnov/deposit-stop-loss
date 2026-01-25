using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application.ExternalServices;
using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Infrastructure.ExternalServices;

/// <summary>
///     National Bank of Georgia exchange rate provider.
///     Free API, no authentication required.
///     https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/en/json/
/// </summary>
public sealed class NbgExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;

    public NbgExchangeRateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string ProviderCode => "NBG";

    public Task<ExternalExchangeRate?> GetCurrentRateAsync(
        string fromCurrency,
        string toCurrency,
        RateType rateType,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement NBG API call
        // NBG only has official rates, not commercial/concept
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
        // NBG supports historical rates: ?date=2024-01-15
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

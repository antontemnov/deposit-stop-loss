using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application.ExternalServices;
using DepositStopLoss.Domain.SharedKernel;

using Microsoft.Extensions.Logging;
using NodaTime;

namespace DepositStopLoss.Infrastructure.ExternalServices;

/// <summary>
///     Composite provider that tries multiple providers in order (fallback chain).
/// </summary>
public sealed partial class CompositeExchangeRateProvider : IExchangeRateProvider
{
    private readonly ILogger<CompositeExchangeRateProvider> _logger;

    private readonly IEnumerable<IExchangeRateProvider> _providers;

    public CompositeExchangeRateProvider(IEnumerable<IExchangeRateProvider> providers, ILogger<CompositeExchangeRateProvider> logger)
    {
        _providers = providers;
        _logger = logger;
    }

    public string ProviderCode => "Composite";

    public async Task<ExternalExchangeRate?> GetCurrentRateAsync(
        string fromCurrency,
        string toCurrency,
        RateType rateType,
        CancellationToken cancellationToken = default)
    {
        foreach (IExchangeRateProvider provider in _providers)
        {
            try
            {
                ExternalExchangeRate? rate = await provider.GetCurrentRateAsync(fromCurrency, toCurrency, rateType, cancellationToken);

                if (rate is not null)
                {
                    return rate;
                }
            }
            catch (System.Exception ex)
            {
                LogProviderFailed(ex, provider.ProviderCode, fromCurrency, toCurrency);
            }
        }

        return null;
    }

    public async Task<ExternalExchangeRate?> GetHistoricalRateAsync(
        string fromCurrency,
        string toCurrency,
        LocalDate date,
        RateType rateType,
        CancellationToken cancellationToken = default)
    {
        foreach (IExchangeRateProvider provider in _providers)
        {
            try
            {
                ExternalExchangeRate? rate = await provider.GetHistoricalRateAsync(
                    fromCurrency,
                    toCurrency,
                    date,
                    rateType,
                    cancellationToken);

                if (rate is not null)
                {
                    return rate;
                }
            }
            catch (System.Exception ex)
            {
                LogProviderFailedHistorical(ex, provider.ProviderCode, fromCurrency, toCurrency, date);
            }
        }

        return null;
    }

    public Task<IReadOnlyList<ExternalExchangeRate>> GetHistoricalRatesAsync(
        string fromCurrency,
        string toCurrency,
        LocalDate from,
        LocalDate to,
        RateType rateType,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement with fallback logic
        throw new System.NotSupportedException();
    }

    [LoggerMessage(Level = LogLevel.Warning, Message = "Provider {ProviderCode} failed for {From}/{To}")]
    private partial void LogProviderFailed(System.Exception ex, string providerCode, string from, string to);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Provider {ProviderCode} failed for historical {From}/{To} on {Date}")]
    private partial void LogProviderFailedHistorical(System.Exception ex, string providerCode, string from, string to, LocalDate date);
}

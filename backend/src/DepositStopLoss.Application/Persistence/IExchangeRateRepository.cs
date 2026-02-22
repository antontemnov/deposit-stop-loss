using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Domain.Banking;
using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Application.Persistence;

/// <summary>
///     Repository interface for ExchangeRate entity.
/// </summary>
public interface IExchangeRateRepository
{
    Task AddAsync(ExchangeRate rate, CancellationToken cancellationToken = default);

    Task AddRangeAsync(IEnumerable<ExchangeRate> rates, CancellationToken cancellationToken = default);

    Task<ExchangeRate?> GetLatestRateAsync(
        BankIdentity bankId,
        Currency fromCurrency,
        Currency toCurrency,
        RateType rateType,
        CancellationToken cancellationToken = default);

    Task<ExchangeRate?> GetRateForDateAsync(
        BankIdentity bankId,
        Currency fromCurrency,
        Currency toCurrency,
        LocalDate date,
        RateType rateType,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ExchangeRate>> GetRatesInRangeAsync(
        BankIdentity bankId,
        Currency fromCurrency,
        Currency toCurrency,
        LocalDate startDate,
        LocalDate endDate,
        RateType rateType,
        CancellationToken cancellationToken = default);
}

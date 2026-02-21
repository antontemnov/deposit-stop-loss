using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application.Persistence;
using DepositStopLoss.Domain.Banking;
using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Infrastructure.Data.Repositories;

/// <summary>
///     EF Core implementation of IExchangeRateRepository.
/// </summary>
public sealed class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly ApplicationDbContext _context;

    public ExchangeRateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task AddAsync(ExchangeRate rate, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task AddRangeAsync(IEnumerable<ExchangeRate> rates, CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task<ExchangeRate?> GetLatestRateAsync(
        BankIdentity bankId,
        Currency fromCurrency,
        Currency toCurrency,
        RateType rateType,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task<ExchangeRate?> GetRateForDateAsync(
        BankIdentity bankId,
        Currency fromCurrency,
        Currency toCurrency,
        LocalDate date,
        RateType rateType,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }

    public Task<IReadOnlyList<ExchangeRate>> GetRatesInRangeAsync(
        BankIdentity bankId,
        Currency fromCurrency,
        Currency toCurrency,
        LocalDate startDate,
        LocalDate endDate,
        RateType rateType,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement
        throw new NotSupportedException();
    }
}

using System;

using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Domain.Banking;

/// <summary>
///     Exchange rate entity. Stores historical exchange rates from banks.
/// </summary>
public sealed class ExchangeRate : Entity<ExchangeRateIdentity>
{
    private ExchangeRate()
    {
    }

    private ExchangeRate(
        ExchangeRateIdentity id,
        BankIdentity bankId,
        Currency fromCurrency,
        Currency toCurrency,
        decimal buyRate,
        decimal sellRate,
        LocalDate rateDate,
        RateType rateType,
        Instant fetchedAt)
        : base(id)
    {
        BankId = bankId;
        FromCurrency = fromCurrency;
        ToCurrency = toCurrency;
        BuyRate = buyRate;
        SellRate = sellRate;
        RateDate = rateDate;
        RateType = rateType;
        FetchedAt = fetchedAt;
    }

    /// <summary>
    ///     Bank that provided this rate.
    /// </summary>
    public BankIdentity BankId { get; private set; }

    /// <summary>
    ///     Bank buys foreign currency at this rate.
    /// </summary>
    public decimal BuyRate { get; private set; }

    /// <summary>
    ///     When we fetched this rate from API.
    /// </summary>
    public Instant FetchedAt { get; }

    /// <summary>
    ///     Source currency (e.g., USD).
    /// </summary>
    public Currency FromCurrency { get; private set; }

    /// <summary>
    ///     Date this rate is valid for.
    /// </summary>
    public LocalDate RateDate { get; private set; }

    /// <summary>
    ///     Type of rate (Commercial or Concept/Discounted).
    /// </summary>
    public RateType RateType { get; private set; }

    /// <summary>
    ///     Bank sells foreign currency at this rate (usually higher).
    /// </summary>
    public decimal SellRate { get; private set; }

    /// <summary>
    ///     Target currency (e.g., GEL).
    /// </summary>
    public Currency ToCurrency { get; private set; }

    /// <summary>
    ///     Factory method to create exchange rate.
    /// </summary>
    public static ExchangeRate Create(
        BankIdentity bankId,
        Currency fromCurrency,
        Currency toCurrency,
        decimal buyRate,
        decimal sellRate,
        LocalDate rateDate,
        RateType rateType)
    {
        if (buyRate <= 0)
        {
            throw new ArgumentException("Buy rate must be positive", nameof(buyRate));
        }

        if (sellRate <= 0)
        {
            throw new ArgumentException("Sell rate must be positive", nameof(sellRate));
        }

        return new ExchangeRate(
            ExchangeRateIdentity.New(),
            bankId,
            fromCurrency,
            toCurrency,
            buyRate,
            sellRate,
            rateDate,
            rateType,
            SystemClock.Instance.GetCurrentInstant());
    }

    /// <summary>
    ///     Check if rate is expired (older than given duration).
    /// </summary>
    public bool IsExpired(Duration maxAge)
    {
        Instant now = SystemClock.Instance.GetCurrentInstant();
        Duration age = now - FetchedAt;

        return age > maxAge;
    }
}

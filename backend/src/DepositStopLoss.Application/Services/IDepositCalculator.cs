using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Application.Services;

/// <summary>
///     Service for calculating deposit profitability and accrued interest.
/// </summary>
public interface IDepositCalculator
{
    /// <summary>
    ///     Full calculation result with all metrics.
    /// </summary>
    DepositCalculationResult Calculate(Deposit deposit, decimal currentExchangeRate, Instant asOfDate);

    /// <summary>
    ///     Calculate accrued interest for deposit up to specific date.
    /// </summary>
    Money CalculateAccruedInterest(Deposit deposit, Instant asOfDate);

    /// <summary>
    ///     Calculate current value of deposit in USD.
    /// </summary>
    Money CalculateCurrentValueInUsd(Deposit deposit, decimal currentExchangeRate, Instant asOfDate);

    /// <summary>
    ///     Calculate profit/loss percentage in USD compared to initial investment.
    /// </summary>
    Percentage CalculateProfitLossPercent(Deposit deposit, decimal currentExchangeRate, Instant asOfDate);
}

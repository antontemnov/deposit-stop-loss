using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.Notifications;
using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Application.Services;

/// <summary>
///     Implementation of deposit calculator.
///     STUB: Basic implementation, will be enhanced with full logic later.
/// </summary>
public sealed class DepositCalculator : IDepositCalculator
{
    public DepositCalculationResult Calculate(Deposit deposit, decimal currentExchangeRate, Instant asOfDate)
    {
        // TODO: Implement full calculation with notification logic
        Money accruedInterest = CalculateAccruedInterest(deposit, asOfDate);
        Money totalInDepositCurrency = deposit.CurrentAmount.Add(accruedInterest);
        Money totalInUsd = CalculateCurrentValueInUsd(deposit, currentExchangeRate, asOfDate);
        Percentage profitLossPercent = CalculateProfitLossPercent(deposit, currentExchangeRate, asOfDate);

        // Determine if should notify
        (bool shouldNotify, NotificationLevel? notificationLevel) = DetermineNotificationLevel(deposit, profitLossPercent);

        return new DepositCalculationResult
        {
            TotalAmountInDepositCurrency = totalInDepositCurrency,
            TotalAmountInUsd = totalInUsd,
            AccruedInterest = accruedInterest,
            ProfitLossPercent = profitLossPercent,
            ExchangeRate = currentExchangeRate,
            CalculatedAt = asOfDate,
            ShouldNotify = shouldNotify,
            NotificationLevel = notificationLevel,
        };
    }

    public Money CalculateAccruedInterest(Deposit deposit, Instant asOfDate)
    {
        // TODO: Implement accrued interest calculation
        // Formula: principal * (rate / 100) * (days / 365)
        double daysSinceOpened = (asOfDate - deposit.OpenedAt).TotalDays;

        if (daysSinceOpened <= 0)
        {
            return Money.Zero(deposit.InitialAmount.Currency);
        }

        decimal annualRate = deposit.AnnualInterestRate.ToFraction();
        Money interest = deposit.CurrentAmount.MultiplyBy(annualRate * (decimal)(daysSinceOpened / 365.0));

        return interest;
    }

    public Money CalculateCurrentValueInUsd(Deposit deposit, decimal currentExchangeRate, Instant asOfDate)
    {
        // TODO: Implement USD conversion
        // Get total amount (initial + accrued interest) and convert to USD
        Money accruedInterest = CalculateAccruedInterest(deposit, asOfDate);
        Money totalInDepositCurrency = deposit.CurrentAmount.Add(accruedInterest);

        return totalInDepositCurrency.ConvertTo(Currency.Usd, currentExchangeRate);
    }

    public Percentage CalculateProfitLossPercent(Deposit deposit, decimal currentExchangeRate, Instant asOfDate)
    {
        // TODO: Implement profit/loss calculation
        // Compare current USD value vs weighted average initial USD value
        Money currentValueUsd = CalculateCurrentValueInUsd(deposit, currentExchangeRate, asOfDate);

        // Get weighted average initial exchange rate
        decimal avgInitialRate = deposit.CalculateWeightedAverageExchangeRate();
        Money initialValueUsd = deposit.CurrentAmount.ConvertTo(Currency.Usd, avgInitialRate);

        if (initialValueUsd.Amount == 0)
        {
            return Percentage.Zero;
        }

        Money profitUsd = currentValueUsd.Subtract(initialValueUsd);
        decimal profitPercent = (profitUsd.Amount / initialValueUsd.Amount) * 100;

        return Percentage.FromValue(profitPercent);
    }

    private static (bool ShouldNotify, NotificationLevel? Level) DetermineNotificationLevel(Deposit deposit, Percentage profitLossPercent)
    {
        // Level 3: CRITICAL - Profitability ≤ 0%
        if (profitLossPercent.Value <= 0)
        {
            return (true, NotificationLevel.Critical);
        }

        // Level 2: WARNING - Close to stop-loss (within 2-4%)
        decimal distanceToStopLoss = profitLossPercent.Value - deposit.StopLossThreshold.Value;

        if ((distanceToStopLoss <= 4) && (distanceToStopLoss > 0))
        {
            return (true, NotificationLevel.Warning);
        }

        // Level 1: INFO - Below expected rate
        if (profitLossPercent.Value < deposit.AnnualInterestRate.Value)
        {
            return (true, NotificationLevel.Info);
        }

        return (false, null);
    }
}

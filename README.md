# Deposit Stop Loss

Telegram bot with web dashboard for monitoring GEL deposit profitability against USD exchange rate fluctuations.

## The Problem

You have a bank deposit in Georgian Lari (GEL) earning 12% annual interest. The exchange rate when you opened the deposit was **1 USD = 2.85 GEL**.

After 6 months:
- Your deposit earned **+6% in GEL** (half of 12% annual rate)
- But the exchange rate dropped to **1 USD = 3.00 GEL** (GEL weakened by 5.3%)
- **Result**: When converting back to USD, you're only **+0.7% in profit** instead of expected +6%

If the rate continues to drop, you could end up at **break-even (0% profit)** or even **negative**, despite earning interest.

## The Solution

This system:

1. **Monitors exchange rates** - Fetches USD/GEL rates from TBC Bank API every 10 minutes
2. **Calculates real profitability** - Computes your deposit value in USD terms, accounting for both interest earned and exchange rate changes
3. **Sends smart notifications** - Alerts you when profitability approaches your stop-loss threshold (e.g., when you're close to 0% profit in USD)
4. **Tracks history** - Stores snapshots of your deposit's performance over time

**Example notification**:
```
⚠️ Warning: Deposit #1 approaching break-even

Current profit in USD: +0.5%
Your stop-loss threshold: 0%

Exchange rate: 1 USD = 2.98 GEL
Accrued interest: +5.8% (GEL)

Consider closing the deposit soon.
```

## Technology Stack

**Backend**: .NET 10 (Clean Architecture + CQRS + DDD)
**Frontend**: Angular 19 (planned)
**Database**: PostgreSQL (Supabase)
**Bot**: Telegram Bot API
**Jobs**: Hangfire (rate monitoring, notifications)
**Caching**: Redis

## Getting Started

```bash
# Build and run
cd backend
dotnet restore
dotnet build
cd src/DepositStopLoss.Api
dotnet run
```

## Telegram Bot Commands

- `/start` - Register and start
- `/add_deposit` - Add deposit with guided wizard
- `/my_deposits` - View all deposits with profitability
- `/current_rate` - Check USD/GEL exchange rate
- `/settings` - Configure notifications

## License

Private project

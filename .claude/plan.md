# Deposit Stop Loss — План реализации

## Обзор проекта

**Цель**: Telegram бот + веб-панель для контроля просадки депозита в иностранной валюте (лари) относительно доллара, с уведомлениями при приближении к зоне безубыточности.

**Приоритет интерфейсов**:

| Интерфейс | Приоритет | Назначение |
|-----------|-----------|------------|
| **Telegram Bot** | Primary | Все операции CRUD, уведомления |
| **Telegram Mini App** | Secondary | Исторические графики (Angular Web App в Telegram) |
| **Web Dashboard** | Optional | Для продвинутых пользователей |

---

## Технологический стек

### Backend

| Технология | Версия | Назначение |
|------------|--------|------------|
| .NET | 10 | Runtime + SDK |
| FastEndpoints | 7.2+ | CQRS endpoints, Vertical Slices, AOT-ready |
| EF Core + Npgsql | 10 | ORM + PostgreSQL |
| Npgsql.NodaTime | 10 | NodaTime type mapping для PostgreSQL |
| FluentValidation | 12+ | Валидация commands/queries |
| ErrorOr | 2+ | Result Pattern |
| Riok.Mapperly | 4+ | Compile-time DTO mapping (Source Generators) |
| NodaTime | 3+ | Date/Time + IClock injection |
| Quartz.NET | 3.15+ | Background Jobs |
| Telegram.Bot | 22+ | Telegram Bot API |
| StackExchange.Redis | — | Кэширование, FSM state |
| Serilog | 10+ | Structured logging |
| OpenTelemetry | 1.11+ | Observability (traces, metrics) |
| MailKit | — | Email уведомления админу |

### Frontend

| Технология | Назначение |
|------------|------------|
| Angular 19+ | Telegram Mini App + Web Dashboard |
| @angular/localize | i18n (нативная Angular локализация) |
| Chart.js + ng2-charts | Графики |
| PrimeNG или Angular Material | UI компоненты |

### Infrastructure

| Технология | Назначение |
|------------|------------|
| PostgreSQL (Supabase managed) | База данных |
| Redis 7 | Кэш + FSM state |
| Docker + Docker Compose | Контейнеризация |
| GitHub Actions | CI/CD |
| Hetzner VPS | Deployment |
| Let's Encrypt | SSL |

### Testing

| Технология | Назначение |
|------------|------------|
| xUnit | Test framework |
| NSubstitute | Mocking |
| FluentAssertions | Readable assertions |
| Bogus | Test data generation |
| Testcontainers | Docker containers для integration tests |
| Microsoft.AspNetCore.Mvc.Testing | API integration tests |

### NuGet пакеты по проектам

```
Domain:
  - NodaTime

Application:
  - FastEndpoints (ICommand, ICommandHandler, IQuery, IQueryHandler)
  - FluentValidation + FluentValidation.DependencyInjectionExtensions
  - ErrorOr
  - Riok.Mapperly
  - NodaTime
  - Microsoft.Extensions.DependencyInjection.Abstractions

Infrastructure:
  - Npgsql.EntityFrameworkCore.PostgreSQL
  - Npgsql.EntityFrameworkCore.PostgreSQL.NodaTime
  - Quartz + Quartz.Extensions.Hosting
  - Telegram.Bot
  - StackExchange.Redis
  - MailKit

Api:
  - FastEndpoints + FastEndpoints.Swagger
  - Serilog.AspNetCore + Sinks (Console, File)
  - NodaTime.Serialization.SystemTextJson
  - OpenTelemetry (AspNetCore, Http, OTLP)
  - Microsoft.AspNetCore.Authentication.JwtBearer

Tests:
  - xUnit + xunit.runner.visualstudio
  - NSubstitute
  - FluentAssertions
  - Bogus
  - Testcontainers
  - Microsoft.AspNetCore.Mvc.Testing
```

---

## Архитектурные паттерны

### Clean Architecture

```
┌─────────────────────────────────────────────┐
│                   Api                        │  ← FastEndpoints, Middleware
│  ┌─────────────────────────────────────────┐ │
│  │            Application                   │ │  ← CQRS Commands/Queries, Services
│  │  ┌─────────────────────────────────────┐ │ │
│  │  │             Domain                   │ │ │  ← Entities, Value Objects, Events
│  │  └─────────────────────────────────────┘ │ │
│  └─────────────────────────────────────────┘ │
│              Infrastructure                   │  ← EF Core, Quartz, External APIs
└─────────────────────────────────────────────┘
```

Зависимости: Domain ← Application ← Infrastructure, Api

### FastEndpoints CQRS

Используем встроенный **FastEndpoints Command/Query Bus**:
- Commands: операции записи → `ICommand<TResult>`, `ICommandHandler<TCommand, TResult>`
- Queries: операции чтения → `IQuery<TResult>`, `IQueryHandler<TQuery, TResult>`
- Cross-cutting: FastEndpoints **Pre/Post Processors** (валидация, логирование)
- Validation: FastEndpoints + FluentValidation интеграция

### DDD Patterns

- **Aggregate Roots**: Deposit, User (consistency boundaries)
- **Entities**: Bank, ExchangeRate, Notification, DepositSnapshot, DepositContribution, UserBankConnection
- **Value Objects**: Money (ComplexProperty), Percentage (ValueConverter), Currency (enum)
- **Domain Events**: DepositCreated, DepositFundsAdded, DepositClosed, StopLossReached, etc.
- **Strongly Typed IDs**: DepositIdentity, UserIdentity, BankIdentity, etc. (ValueConverter → UUID)

### Value Object EF Core Mapping

| Type | EF Strategy | DB Result |
|------|-------------|-----------|
| `Percentage` | Value Converter | Одна колонка `DECIMAL(5,2)` |
| `Money` | ComplexProperty (EF 8+) | Две колонки: `_amount DECIMAL(18,2)` + `_currency INTEGER` |
| `DepositContribution` | HasMany (Entity с ID) | Отдельная таблица `deposit_contributions` |
| Strongly Typed IDs | Value Converter | `UUID` колонка |
| `Currency` enum | Integer mapping | `INTEGER` колонка |

**Запрет**: JSON columns в БД. Только стандартные колонки.

### Repository + Unit of Work

```csharp
// Application layer
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}

// Infrastructure layer
public sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return await dbContext.SaveChangesAsync(ct);
    }
}
```

Repositories не вызывают `SaveChanges` — это делает UnitOfWork в конце handler-а.

### NodaTime IClock

Domain entities НЕ зависят от `SystemClock.Instance`. Время передаётся через:
- Factory methods: `Deposit.Create(..., Instant openedAt)`
- Application services: `IClock` через DI → передают `Instant` в domain
- Тесты: `FakeClock` для детерминированных результатов

---

## Хранение курсов валют

### Принцип: "Своя БД — источник правды"

```
┌─────────────────┐     Background Job     ┌─────────────────┐
│   External API  │ ──────────────────────► │   PostgreSQL    │
│  (TBC/BOG/NBG)  │   (1-2 раза/сутки)     │  exchange_rates │
└─────────────────┘                         └────────┬────────┘
                                                     │
                                                     ▼
                                              Application
                                            (всегда из БД)
```

**Преимущества**: скорость (<1ms), отказоустойчивость, полная история, аудит.

### Единая таблица exchange_rates

```sql
CREATE TABLE exchange_rates (
    id UUID PRIMARY KEY,
    bank_id UUID NOT NULL REFERENCES banks(id),
    from_currency INTEGER NOT NULL,
    to_currency INTEGER NOT NULL,
    buy_rate DECIMAL(18, 6) NOT NULL,
    sell_rate DECIMAL(18, 6) NOT NULL,
    rate_date DATE NOT NULL,
    rate_type INTEGER NOT NULL,         -- Commercial=1, Discounted=2
    fetched_at TIMESTAMPTZ NOT NULL,
    UNIQUE(bank_id, from_currency, to_currency, rate_date, rate_type)
);
```

### Гранулярность сбора

- **NBG**: 1 раз/день (курс не меняется внутри дня)
- **TBC/BOG**: 2 раза/день (утро + вечер)

### Fallback Chain

```
TBC Deposit:  TBC API → NBG API (fallback) → Cached rate (last resort)
BOG Deposit:  BOG API → NBG API (fallback) → Cached rate (last resort)
```

NBG API — бесплатный, без API Key, без rate limits. Используется ТОЛЬКО как fallback.
При fallback snapshot помечается: `RateSource = NBG_FALLBACK`.

### Backfill (депозит "задним числом")

При создании депозита с датой в прошлом:
1. Ищем данные в своей БД
2. Если нет → запрос к NBG API (поддерживает исторические данные: `?date=YYYY-MM-DD`)
3. Rate Limiting: максимум ~20 запросов за один backfill

### Exchange Rate Provider Abstraction (Strategy Pattern)

```csharp
public interface IExchangeRateProvider
{
    string ProviderCode { get; }  // TBC, BOG, NBG

    Task<ExternalExchangeRate?> GetCurrentRateAsync(
        string fromCurrency, string toCurrency,
        RateType rateType, CancellationToken ct = default);

    Task<ExternalExchangeRate?> GetHistoricalRateAsync(
        string fromCurrency, string toCurrency,
        LocalDate date, RateType rateType,
        CancellationToken ct = default);

    Task<IReadOnlyList<ExternalExchangeRate>> GetHistoricalRatesAsync(
        string fromCurrency, string toCurrency,
        LocalDate from, LocalDate to,
        RateType rateType, CancellationToken ct = default);
}
```

Реализации: `TbcExchangeRateProvider`, `BogExchangeRateProvider`, `NbgExchangeRateProvider`, `CompositeExchangeRateProvider` (fallback chain).

---

## Структура Solution

```
backend/
├── src/
│   ├── DepositStopLoss.Api/                 # ASP.NET Core + FastEndpoints
│   ├── DepositStopLoss.Application/         # CQRS, Services, DTOs
│   ├── DepositStopLoss.Domain/              # Entities, Value Objects, Events
│   ├── DepositStopLoss.Infrastructure/      # EF Core, Quartz, External APIs
│   ├── DepositStopLoss.TelegramBot/         # Telegram Bot handlers
│   └── DepositStopLoss.Shared/              # Shared constants, helpers
├── tests/
│   ├── DepositStopLoss.Application.Tests/   # Unit tests
│   └── DepositStopLoss.Integration.Tests/   # Integration tests
└── deposit-stop-loss.slnx
```

---

## Domain Layer

### Структура

```
DepositStopLoss.Domain/
├── SharedKernel/
│   ├── EntityBase.cs              # Base with domain events
│   ├── Entity<TIdentity>.cs      # Entity with strongly typed ID
│   ├── AggregateRoot<TIdentity>.cs
│   ├── IAggregateRoot.cs
│   ├── IDomainEvent.cs            # Marker interface (Instant OccurredAt)
│   ├── DomainEventBase.cs         # Base record
│   ├── Money.cs                   # Value Object (Amount + Currency)
│   ├── Percentage.cs              # Value Object (decimal Value)
│   ├── Currency.cs                # Enum (GEL, USD, EUR)
│   └── RateType.cs                # Enum (Commercial, Discounted)
├── Deposits/
│   ├── Deposit.cs                 # Aggregate Root
│   ├── DepositIdentity.cs         # Strongly Typed ID
│   ├── DepositContribution.cs     # Entity (внесение в депозит)
│   ├── DepositContributionIdentity.cs
│   ├── DepositStatus.cs           # Enum: Active, Paused, Closed
│   └── DepositSource.cs           # Enum: Manual, TbcApi, BogApi
├── Banking/
│   ├── Bank.cs                    # Entity
│   ├── BankIdentity.cs
│   ├── BankRateSource.cs          # Entity (rate source per bank)
│   ├── BankRateSourceIdentity.cs
│   ├── ExchangeRate.cs            # Entity
│   ├── ExchangeRateIdentity.cs
│   ├── UserBankConnection.cs      # Entity (OAuth)
│   └── UserBankConnectionIdentity.cs
├── Users/
│   ├── User.cs                    # Aggregate Root
│   └── UserIdentity.cs
├── Notifications/
│   ├── Notification.cs            # Entity
│   ├── NotificationIdentity.cs
│   ├── NotificationLevel.cs       # Enum: Info, Warning, Critical
│   └── NotificationStatus.cs      # Enum: Pending, Sent, Failed, Read
├── Snapshots/
│   ├── DepositSnapshot.cs         # Entity (history point)
│   └── DepositSnapshotIdentity.cs
└── Events/
    ├── DepositCreatedEvent.cs
    ├── DepositFundsAddedEvent.cs
    ├── DepositPausedEvent.cs
    ├── DepositResumedEvent.cs
    ├── DepositClosedEvent.cs
    └── StopLossThresholdReachedEvent.cs
```

### Deposit Aggregate — ключевые методы

```csharp
public sealed class Deposit : AggregateRoot<DepositIdentity>
{
    // Factory
    public static Deposit Create(
        UserIdentity userId, BankIdentity bankId,
        Money initialAmount, Percentage annualInterestRate,
        Instant openedAt, int termMonths, RateType rateType,
        Percentage stopLossThreshold, DepositSource source,
        decimal initialExchangeRate);

    // Contribution management
    public void AddFunds(Money amount, decimal exchangeRate, Instant contributedAt);
    public void UpdateContribution(DepositContributionIdentity id,
        Money? amount, Instant? contributedAt, decimal? exchangeRate);
    public void RemoveContribution(DepositContributionIdentity id);
    public decimal CalculateWeightedAverageExchangeRate();

    // Lifecycle
    public void Pause();
    public void Resume();
    public void Close(Instant closedAt);

    // Editing
    public void UpdateDetails(Instant? openedAt, Percentage? rate, int? termMonths, Money? amount);
    public void UpdateStopLossThreshold(Percentage newThreshold);
    public bool CanEditDetails();  // true only for Manual deposits
}
```

### DepositContribution — Entity внутри Aggregate

```csharp
public sealed class DepositContribution : Entity<DepositContributionIdentity>
{
    public Money Amount { get; private set; }
    public Instant ContributedAt { get; private set; }
    public decimal ExchangeRateAtContribution { get; private set; }
    public Money ValueInUsd { get; private set; }

    public static DepositContribution Create(Money amount, Instant contributedAt, decimal exchangeRate);
    internal void Update(Money? amount, Instant? contributedAt, decimal? exchangeRate);
}
```

Редактируется только через методы Deposit aggregate (aggregate boundary).

---

## Application Layer

### Структура

```
DepositStopLoss.Application/
├── Features/                         # Vertical Slices
│   ├── Deposits/
│   │   ├── Commands/
│   │   │   ├── CreateDeposit/
│   │   │   │   ├── CreateDepositCommand.cs
│   │   │   │   ├── CreateDepositCommandHandler.cs
│   │   │   │   └── CreateDepositCommandValidator.cs
│   │   │   ├── AddFunds/
│   │   │   ├── UpdateDeposit/
│   │   │   ├── UpdateContribution/
│   │   │   ├── RemoveContribution/
│   │   │   ├── UpdateStopLoss/
│   │   │   ├── PauseDeposit/
│   │   │   ├── ResumeDeposit/
│   │   │   └── CloseDeposit/
│   │   └── Queries/
│   │       ├── GetUserDeposits/
│   │       ├── GetDepositDetails/
│   │       ├── CalculateProfitability/
│   │       └── GetDepositsTable/
│   ├── ExchangeRates/
│   │   ├── Commands/
│   │   │   └── FetchExchangeRates/
│   │   └── Queries/
│   │       ├── GetCurrentRate/
│   │       └── GetRateHistory/
│   ├── Users/
│   │   ├── Commands/
│   │   │   ├── RegisterUser/
│   │   │   └── UpdateUserSettings/
│   │   └── Queries/
│   │       └── GetUser/
│   ├── Notifications/
│   │   ├── Commands/
│   │   │   ├── CreateNotification/
│   │   │   └── SendPendingNotifications/
│   │   └── Queries/
│   │       └── GetUserNotifications/
│   ├── Banks/
│   │   └── Queries/
│   │       └── GetBanks/
│   └── Admin/
│       └── Commands/
│           └── NotifyAdminError/
├── Services/
│   ├── IDepositCalculator.cs
│   ├── DepositCalculator.cs         # Критичная бизнес-логика
│   └── IAdminNotificationService.cs
├── Persistence/
│   ├── IUnitOfWork.cs
│   ├── IBankRepository.cs
│   ├── IDepositRepository.cs
│   ├── IDepositSnapshotRepository.cs
│   ├── IExchangeRateRepository.cs
│   ├── INotificationRepository.cs
│   └── IUserRepository.cs
├── ExternalServices/
│   ├── IExchangeRateProvider.cs
│   └── ExternalExchangeRate.cs
└── Common/
    └── DTOs/
        ├── DepositDto.cs
        ├── DepositContributionDto.cs
        ├── DepositSnapshotDto.cs
        ├── ExchangeRateDto.cs
        ├── NotificationDto.cs
        ├── UserDto.cs
        └── BankDto.cs
```

### DepositCalculator — критичная бизнес-логика

```csharp
public interface IDepositCalculator
{
    DepositCalculationResult Calculate(Deposit deposit, decimal currentExchangeRate, Instant asOfDate);
    Money CalculateAccruedInterest(Deposit deposit, Instant asOfDate);
    Money CalculateCurrentValueInUsd(Deposit deposit, decimal currentExchangeRate, Instant asOfDate);
    Percentage CalculateProfitLossPercent(Deposit deposit, decimal currentExchangeRate, Instant asOfDate);
}
```

**Формулы**:
- Accrued Interest: `principal * (rate / 100) * (days / 365)` (учитывает contributions с разных дат)
- Current USD Value: `(total_gel + accrued_interest) / sell_rate`
- Profit/Loss %: `((current_usd - initial_usd) / initial_usd) * 100`
- Weighted Average Rate: `Σ(amount_i * rate_i * days_i) / Σ(amount_i * days_i)`

---

## Infrastructure Layer

### Структура

```
DepositStopLoss.Infrastructure/
├── Data/
│   ├── ApplicationDbContext.cs
│   ├── UnitOfWork.cs
│   ├── Configurations/
│   │   ├── BankConfiguration.cs
│   │   ├── DepositConfiguration.cs
│   │   ├── DepositContributionConfiguration.cs
│   │   ├── DepositSnapshotConfiguration.cs
│   │   ├── ExchangeRateConfiguration.cs
│   │   ├── NotificationConfiguration.cs
│   │   ├── UserBankConnectionConfiguration.cs
│   │   └── UserConfiguration.cs
│   └── Repositories/
│       ├── BankRepository.cs
│       ├── DepositRepository.cs
│       ├── DepositSnapshotRepository.cs
│       ├── ExchangeRateRepository.cs
│       ├── NotificationRepository.cs
│       └── UserRepository.cs
├── ExternalServices/
│   ├── TbcExchangeRateProvider.cs
│   ├── BogExchangeRateProvider.cs
│   ├── NbgExchangeRateProvider.cs
│   └── CompositeExchangeRateProvider.cs
├── BackgroundJobs/
│   ├── ExchangeRateFetchJob.cs      # Every 10 min
│   ├── DepositMonitorJob.cs          # Every 5 min
│   ├── NotificationSenderJob.cs      # Every 2 min
│   └── CleanupJob.cs                 # Daily 2:00 AM
└── DependencyInjection.cs
```

### Database Schema

```sql
-- Banks reference table
CREATE TABLE banks (
    id UUID PRIMARY KEY,
    code VARCHAR(10) NOT NULL UNIQUE,
    name VARCHAR(100) NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT true
);

-- Bank Rate Sources (one-to-many from banks)
CREATE TABLE bank_rate_sources (
    id UUID PRIMARY KEY,
    bank_id UUID NOT NULL REFERENCES banks(id) ON DELETE CASCADE,
    rate_type INTEGER NOT NULL,         -- Commercial=1, Discounted=2
    api_url VARCHAR(500) NOT NULL,
    UNIQUE(bank_id, rate_type)
);

-- Users
CREATE TABLE users (
    id UUID PRIMARY KEY,
    telegram_id BIGINT NOT NULL UNIQUE,
    username VARCHAR(100),
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    language_code VARCHAR(10) NOT NULL DEFAULT 'en',
    email VARCHAR(255),
    timezone VARCHAR(50) NOT NULL DEFAULT 'Asia/Tbilisi',
    enable_telegram_notifications BOOLEAN NOT NULL DEFAULT true,
    enable_email_notifications BOOLEAN NOT NULL DEFAULT false,
    is_active BOOLEAN NOT NULL DEFAULT true,
    registered_at TIMESTAMPTZ NOT NULL,
    last_activity_at TIMESTAMPTZ
);

-- Deposits
CREATE TABLE deposits (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL REFERENCES users(id),
    bank_id UUID NOT NULL REFERENCES banks(id),
    initial_amount DECIMAL(18,2) NOT NULL,
    initial_currency INTEGER NOT NULL,
    current_amount DECIMAL(18,2) NOT NULL,
    current_currency INTEGER NOT NULL,
    annual_interest_rate DECIMAL(5,2) NOT NULL,
    stop_loss_threshold DECIMAL(5,2) NOT NULL,
    opened_at TIMESTAMPTZ NOT NULL,
    maturity_date TIMESTAMPTZ NOT NULL,
    closed_at TIMESTAMPTZ,
    term_months INTEGER NOT NULL,
    status INTEGER NOT NULL,          -- Active=1, Paused=2, Closed=3
    source INTEGER NOT NULL,          -- Manual=1, TbcApi=2, BogApi=3
    rate_type INTEGER NOT NULL        -- Commercial=1, Discounted=2
);

-- Deposit Contributions (Entity inside Deposit aggregate)
CREATE TABLE deposit_contributions (
    id UUID PRIMARY KEY,
    deposit_id UUID NOT NULL REFERENCES deposits(id) ON DELETE CASCADE,
    amount DECIMAL(18,2) NOT NULL,
    currency INTEGER NOT NULL,
    contributed_at TIMESTAMPTZ NOT NULL,
    exchange_rate DECIMAL(18,6) NOT NULL,
    value_in_usd_amount DECIMAL(18,2) NOT NULL,
    value_in_usd_currency INTEGER NOT NULL
);

-- Exchange Rates (single table for all providers)
CREATE TABLE exchange_rates (
    id UUID PRIMARY KEY,
    bank_id UUID NOT NULL REFERENCES banks(id),
    from_currency INTEGER NOT NULL,
    to_currency INTEGER NOT NULL,
    buy_rate DECIMAL(18,6) NOT NULL,
    sell_rate DECIMAL(18,6) NOT NULL,
    rate_date DATE NOT NULL,
    rate_type INTEGER NOT NULL,
    fetched_at TIMESTAMPTZ NOT NULL,
    UNIQUE(bank_id, from_currency, to_currency, rate_date, rate_type)
);

-- Deposit Snapshots (historical profitability)
CREATE TABLE deposit_snapshots (
    id UUID PRIMARY KEY,
    deposit_id UUID NOT NULL REFERENCES deposits(id) ON DELETE CASCADE,
    total_deposit_amount DECIMAL(18,2) NOT NULL,
    total_deposit_currency INTEGER NOT NULL,
    total_usd_amount DECIMAL(18,2) NOT NULL,
    total_usd_currency INTEGER NOT NULL,
    accrued_interest_amount DECIMAL(18,2) NOT NULL,
    accrued_interest_currency INTEGER NOT NULL,
    profit_loss_percent DECIMAL(5,2) NOT NULL,
    exchange_rate DECIMAL(18,6) NOT NULL,
    snapshot_date TIMESTAMPTZ NOT NULL
);

-- Notifications
CREATE TABLE notifications (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL REFERENCES users(id),
    deposit_id UUID NOT NULL REFERENCES deposits(id),
    level INTEGER NOT NULL,           -- Info=1, Warning=2, Critical=3
    message VARCHAR(1000) NOT NULL,
    status INTEGER NOT NULL,          -- Pending=1, Sent=2, Failed=3, Read=4
    error_message TEXT,
    created_at TIMESTAMPTZ NOT NULL,
    sent_at TIMESTAMPTZ,
    read_at TIMESTAMPTZ
);

-- User Bank Connections (OAuth tokens)
CREATE TABLE user_bank_connections (
    id UUID PRIMARY KEY,
    user_id UUID NOT NULL REFERENCES users(id),
    bank_id UUID NOT NULL REFERENCES banks(id),
    access_token TEXT NOT NULL,       -- Encrypted (AES-256)
    refresh_token TEXT NOT NULL,      -- Encrypted
    token_expires_at TIMESTAMPTZ NOT NULL,
    scopes TEXT[],
    is_active BOOLEAN NOT NULL DEFAULT true,
    connected_at TIMESTAMPTZ NOT NULL,
    last_sync_at TIMESTAMPTZ,
    UNIQUE(user_id, bank_id)
);

-- Indexes
CREATE INDEX idx_deposits_user_id ON deposits(user_id);
CREATE INDEX idx_deposits_status ON deposits(status);
CREATE INDEX idx_exchange_rates_date ON exchange_rates(rate_date);
CREATE INDEX idx_deposit_snapshots_deposit ON deposit_snapshots(deposit_id, snapshot_date);
CREATE INDEX idx_notifications_user_status ON notifications(user_id, status);
```

---

## API Layer (FastEndpoints)

### Структура

```
DepositStopLoss.Api/
├── Endpoints/
│   ├── Deposits/
│   │   ├── CreateDepositEndpoint.cs      # POST /api/deposits
│   │   ├── GetUserDepositsEndpoint.cs    # GET /api/deposits
│   │   ├── GetDepositDetailsEndpoint.cs  # GET /api/deposits/{id}
│   │   ├── UpdateDepositEndpoint.cs      # PUT /api/deposits/{id}
│   │   ├── PauseDepositEndpoint.cs       # PATCH /api/deposits/{id}/pause
│   │   ├── ResumeDepositEndpoint.cs      # PATCH /api/deposits/{id}/resume
│   │   ├── CloseDepositEndpoint.cs       # PATCH /api/deposits/{id}/close
│   │   ├── UpdateStopLossEndpoint.cs     # PATCH /api/deposits/{id}/stop-loss
│   │   ├── AddFundsEndpoint.cs           # POST /api/deposits/{id}/contributions
│   │   ├── UpdateContributionEndpoint.cs # PUT /api/deposits/{id}/contributions/{cid}
│   │   ├── RemoveContributionEndpoint.cs # DELETE /api/deposits/{id}/contributions/{cid}
│   │   └── CalculateEndpoint.cs          # GET /api/deposits/{id}/profitability
│   ├── ExchangeRates/
│   │   ├── GetCurrentRateEndpoint.cs     # GET /api/exchange-rates/current
│   │   └── GetRateHistoryEndpoint.cs     # GET /api/exchange-rates/history
│   ├── Notifications/
│   │   ├── GetNotificationsEndpoint.cs   # GET /api/notifications
│   │   └── MarkAsReadEndpoint.cs         # PATCH /api/notifications/{id}/read
│   ├── Banks/
│   │   └── GetBanksEndpoint.cs           # GET /api/banks
│   ├── Users/
│   │   └── UpdateSettingsEndpoint.cs     # PATCH /api/users/{id}/settings
│   ├── BankConnections/
│   │   ├── InitConnectionEndpoint.cs     # POST /api/bank-connections
│   │   ├── OAuthCallbackEndpoint.cs      # GET /api/bank-connections/callback
│   │   └── DisconnectEndpoint.cs         # DELETE /api/bank-connections/{id}
│   └── Webhook/
│       └── TelegramWebhookEndpoint.cs    # POST /api/webhook/telegram
├── Middleware/
│   ├── ExceptionHandlingMiddleware.cs
│   └── TelegramWebhookValidation.cs
└── Program.cs
```

### Program.cs конфигурация

```csharp
var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((ctx, config) => config.ReadFrom.Configuration(ctx.Configuration));

// DI
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// FastEndpoints
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

// Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer();

// Redis
builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Redis"));

// OpenTelemetry
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter());

// Health Checks
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen();
app.MapHealthChecks("/health");

app.Run();
```

---

## Telegram Bot

### Структура

```
DepositStopLoss.TelegramBot/
├── BotService.cs                    # Webhook/Polling setup
├── Commands/
│   ├── StartCommand.cs              # /start
│   ├── MenuCommand.cs               # /menu
│   ├── AddDepositCommand.cs         # /add_deposit (FSM)
│   ├── MyDepositsCommand.cs         # /my_deposits
│   ├── CurrentRateCommand.cs        # /current_rate
│   ├── SettingsCommand.cs           # /settings
│   ├── ConnectBankCommand.cs        # /connect_bank
│   └── HelpCommand.cs              # /help
├── Handlers/
│   ├── UpdateHandler.cs             # Main router
│   ├── MessageHandler.cs            # Text messages (FSM)
│   └── CallbackQueryHandler.cs      # Inline buttons
├── Keyboards/
│   ├── MainMenuKeyboard.cs
│   ├── DepositActionsKeyboard.cs
│   └── SettingsKeyboard.cs
└── States/
    └── UserConversationState.cs     # FSM state (Redis)
```

### Conversation State (FSM)

Пошаговое создание депозита:
1. SelectBank → 2. EnterAmount → 3. EnterInterestRate → 4. EnterStartDate
→ 5. EnterTerm → 6. SelectRateType → 7. EnterStopLoss → 8. Confirm

FSM state хранится в Redis: `user:{telegram_id}:state`

### Формат уведомлений

| Уровень | Условие | Emoji | Cooldown |
|---------|---------|-------|----------|
| INFO | Profit < expected rate | 📉 | 24h |
| WARNING | До stop-loss 2-4% | ⚠️ | 12h |
| CRITICAL | Profit ≤ 0% | 🚨 | 1h |

---

## TBC ID OAuth 2.0 Integration

### Flow

```
User → /connect_bank → Bot generates OAuth URL
  → User authorizes in TBC ID (browser)
  → TBC redirects with authorization_code
  → Backend exchanges code → access_token + refresh_token
  → GET /v2/savings → list deposits
  → User selects which to import
```

### TBC API Endpoints

```
GET /v2/savings                     — List all deposits
GET /v2/savings/{resourceId}        — Deposit details
GET /v2/savings/{resourceId}/balances — Deposit balance
```

### Модель OAuth

Токены шифруются AES-256 через `IDataProtector` перед сохранением в БД.
Refresh token автоматически обновляется при истечении access token.

---

## Background Jobs (Quartz.NET)

| Job | Расписание | Описание |
|-----|-----------|----------|
| `ExchangeRateFetchJob` | */10 * * * * (10 min) | Fetch rates from bank APIs, save to DB |
| `DepositMonitorJob` | */5 * * * * (5 min) | Calculate profitability, create snapshots, trigger notifications |
| `NotificationSenderJob` | */2 * * * * (2 min) | Send pending notifications via Telegram |
| `CleanupJob` | 0 2 * * * (daily 2AM) | Delete old notifications (30d), trim snapshots, trim rates (2y) |

---

## Уведомления (3 уровня)

### Trigger Logic

```csharp
// Level 3: CRITICAL — Profit ≤ 0%
if (profitPercent <= 0) → notify (cooldown 1h)

// Level 2: WARNING — Approaching stop-loss (within 2-4%)
if (distanceToStopLoss <= 4 && distanceToStopLoss > 0) → notify (cooldown 12h)

// Level 1: INFO — Below expected rate
if (profitPercent < expectedRate) → notify (cooldown 24h)
```

### Admin Notifications (отдельный бот)

- Unhandled exceptions → Telegram + Email
- API provider failures → Telegram
- Содержание: error message, stack trace, timestamp, traceId

---

## Security

| Аспект | Решение |
|--------|---------|
| OAuth токены | AES-256 encryption at rest (IDataProtector) |
| API Keys | User Secrets (dev) / Env Vars (prod) |
| HTTPS | Обязательно, HSTS, TLS 1.2+ |
| Rate Limiting | 100 req/min per user |
| Input Validation | FluentValidation |
| SQL Injection | EF Core parameterization |
| Telegram Webhook | X-Telegram-Bot-Api-Secret-Token validation |
| JWT | Short-lived (15 min) + refresh token |

---

## Timezone Strategy

- **Сервер**: всё в UTC (TIMESTAMPTZ, NodaTime Instant)
- **Клиент**: локальное время пользователя
- **Default**: Asia/Tbilisi (GMT+4)
- **Получение timezone**: User settings → Telegram API → fallback "Asia/Tbilisi"

---

## Локализация (i18n)

| Код | Язык | Статус |
|-----|------|--------|
| en | English | MVP |
| ru | Русский | MVP |
| ka | ქართული | v1.1 |

- **Backend**: `IStringLocalizer<Messages>` + JSON resource files
- **Frontend**: `@angular/localize` (Angular native i18n)
- **Telegram Bot**: язык из `User.LanguageCode`

---

## Конфигурация

### appsettings.json (НЕ секреты)

```json
{
  "BankApis": {
    "Tbc": {
      "BaseUrl": "https://api.tbcbank.ge",
      "CommercialEndpoint": "/v1/exchange-rates/commercial",
      "DiscountedEndpoint": "/v1/exchange-rates/concept"
    },
    "Nbg": {
      "BaseUrl": "https://nbg.gov.ge/gw/api/ct/monetarypolicy",
      "CurrenciesEndpoint": "/currencies/en/json"
    }
  },
  "Quartz": { "WorkerCount": 5 },
  "Caching": { "RateCacheMinutes": 5 }
}
```

### Секреты

**Development**: `dotnet user-secrets`
**Production**: Environment Variables (`TELEGRAM__BOTTOKEN`, `DATABASE__CONNECTIONSTRING`, etc.)

---

## Docker & CI/CD

### Docker Compose

```yaml
services:
  api:
    build: { context: ., dockerfile: docker/Dockerfile.api }
    ports: ["5000:8080"]
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=${DB_CONNECTION_STRING}
      - Telegram__BotToken=${TELEGRAM_BOT_TOKEN}
    depends_on: [redis]

  redis:
    image: redis:7-alpine
    ports: ["6379:6379"]
    volumes: [redis-data:/data]

  frontend:
    build: { context: ., dockerfile: docker/Dockerfile.frontend }
    ports: ["80:80", "443:443"]
    depends_on: [api]
```

### GitHub Actions

- **CI** (push to main/develop, PR): build + test (backend + frontend)
- **CD** (push to main): Docker build → push → SSH deploy to Hetzner VPS

### Git Flow

- `main` — production (deploy на VPS)
- `develop` — активная разработка (основная рабочая ветка)
- `feature/*` — feature branches
- `hotfix/*` — срочные исправления

---

## Roadmap

### Фаза 1: MVP Backend (2 недели)
1. ~~Создать solution structure~~ ✅
2. ~~Domain models~~ ✅
3. ~~Application CQRS skeleton~~ ✅ (stubs)
4. ~~Infrastructure skeleton~~ ✅ (stubs)
5. **→ Исправить EF Value Object mapping (ComplexProperty, ValueConverter)**
6. **→ Реализовать repositories (EF Core)**
7. **→ Реализовать command/query handlers**
8. **→ Реализовать DepositCalculator (core logic)**
9. **→ Реализовать exchange rate providers (TBC, NBG)**
10. **→ Настроить Quartz background jobs**
11. **→ Настроить FastEndpoints + Program.cs**
12. **→ Unit tests для DepositCalculator**

### Фаза 2: Telegram Bot (1-2 недели)
1. BotService (Webhook/Polling)
2. Основные команды (/start, /add_deposit, /my_deposits)
3. FSM для пошагового добавления
4. Inline keyboards
5. Уведомления

### Фаза 3: Frontend — Telegram Mini App (1-2 недели)
1. Angular проект
2. График доходности (Chart.js)
3. Telegram WebApp SDK интеграция

### Фаза 4: DevOps (1 неделя)
1. Dockerfiles
2. docker-compose
3. GitHub Actions CI/CD
4. Hetzner VPS deployment
5. SSL (Let's Encrypt)

### Фаза 5: Polish (1 неделя)
1. Integration tests
2. E2E tests
3. Security audit
4. Performance testing

---

## Wishlist (будущее)

### v1.1
- Грузинский язык (ქართული)
- Email уведомления
- Экспорт (CSV, Excel, PDF)

### v2.0
- Bank of Georgia, Liberty Bank
- EUR/GEL пара
- Demo режим
- Частичное снятие (withdrawal)
- Автоуведомление при MaturityDate

### v3.0
- Premium подписка (unlimited deposits, custom alerts)
- AI recommendations ("курс на минимуме — хороший момент для депозита")
- Платежи: TON, BOG Pay, Stripe

---

## Ресурсы

**API курсов**:
- TBC Bank: https://developers.tbcbank.ge
- Bank of Georgia: https://api.bog.ge
- NBG: https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/en/json/

**Telegram**:
- Bot API: https://core.telegram.org/bots/api
- Mini Apps: https://core.telegram.org/bots/webapps

**Библиотеки**:
- FastEndpoints: https://fast-endpoints.com
- ErrorOr: https://github.com/amantinband/error-or
- Riok.Mapperly: https://mapperly.riok.app
- Quartz.NET: https://www.quartz-scheduler.net
- Telegram.Bot: https://github.com/TelegramBots/Telegram.Bot

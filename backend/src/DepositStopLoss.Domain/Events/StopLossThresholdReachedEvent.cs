using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Domain.Events;

/// <summary>
///     Domain event: Deposit profitability reached stop-loss threshold.
/// </summary>
public sealed record StopLossThresholdReachedEvent(
    DepositIdentity DepositId,
    Percentage CurrentProfitability,
    Percentage StopLossThreshold,
    Instant OccurredAt) : DomainEventBase(OccurredAt);

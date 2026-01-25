using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Domain.Events;

/// <summary>
///     Domain event: Funds were added to deposit.
/// </summary>
public sealed record DepositFundsAddedEvent(DepositIdentity DepositId, Money AmountAdded, decimal ExchangeRate, Instant OccurredOn)
    : DomainEventBase(OccurredOn);

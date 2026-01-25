using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Domain.Events;

/// <summary>
///     Domain event: Deposit was closed.
/// </summary>
public sealed record DepositClosedEvent(DepositIdentity DepositId, Instant ClosedAt, Instant OccurredOn) : DomainEventBase(OccurredOn);

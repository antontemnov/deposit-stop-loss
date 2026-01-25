using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Domain.Events;

/// <summary>
///     Domain event: Deposit monitoring was paused.
/// </summary>
public sealed record DepositPausedEvent(DepositIdentity DepositId, Instant OccurredOn) : DomainEventBase(OccurredOn);

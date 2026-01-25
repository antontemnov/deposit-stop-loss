using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.SharedKernel;

using NodaTime;

namespace DepositStopLoss.Domain.Events;

/// <summary>
///     Domain event: Deposit monitoring was resumed.
/// </summary>
public sealed record DepositResumedEvent(DepositIdentity DepositId, Instant OccurredOn) : DomainEventBase(OccurredOn);

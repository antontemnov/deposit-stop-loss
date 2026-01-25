using DepositStopLoss.Domain.Deposits;
using DepositStopLoss.Domain.SharedKernel;
using DepositStopLoss.Domain.Users;

using NodaTime;

namespace DepositStopLoss.Domain.Events;

/// <summary>
///     Domain event: Deposit was created.
/// </summary>
public sealed record DepositCreatedEvent(DepositIdentity DepositId, UserIdentity UserId, Instant OccurredOn) : DomainEventBase(OccurredOn);

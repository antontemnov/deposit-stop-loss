using System;

using ErrorOr;

using FastEndpoints;

using NodaTime;

namespace DepositStopLoss.Application.Features.Deposits.Commands.AddFunds;

/// <summary>
///     Command to add funds to an existing deposit.
/// </summary>
public sealed record AddFundsCommand(Guid DepositId, decimal Amount, decimal ExchangeRateAtContribution, Instant ContributedAt)
    : ICommand<ErrorOr<Success>>;

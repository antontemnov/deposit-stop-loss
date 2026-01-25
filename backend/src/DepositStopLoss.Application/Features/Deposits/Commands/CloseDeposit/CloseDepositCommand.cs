using System;

using DepositStopLoss.Application;

using ErrorOr;
using NodaTime;

namespace DepositStopLoss.Application.Features.Deposits.Commands.CloseDeposit;

/// <summary>
///     Command to close a deposit.
/// </summary>
public sealed record CloseDepositCommand(Guid DepositId, Instant ClosedAt) : ICommand<ErrorOr<Success>>;

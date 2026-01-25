using System;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;
using NodaTime;

namespace DepositStopLoss.Application.Features.Deposits.Commands.CreateDeposit;

/// <summary>
///     Command to create a new deposit.
/// </summary>
public sealed record CreateDepositCommand(
    Guid UserId,
    Guid BankId,
    decimal InitialAmount,
    string Currency,
    decimal AnnualInterestRate,
    int TermMonths,
    Instant OpenedAt,
    Instant MaturityDate,
    string RateType,
    decimal StopLossThreshold) : ICommand<ErrorOr<DepositDto>>;

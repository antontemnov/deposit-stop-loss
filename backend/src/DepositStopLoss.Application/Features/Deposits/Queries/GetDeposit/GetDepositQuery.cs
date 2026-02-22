using System;

using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

using FastEndpoints;

namespace DepositStopLoss.Application.Features.Deposits.Queries.GetDeposit;

/// <summary>
///     Query to get a deposit by Id.
/// </summary>
public sealed record GetDepositQuery(Guid DepositId) : ICommand<ErrorOr<DepositDto>>;

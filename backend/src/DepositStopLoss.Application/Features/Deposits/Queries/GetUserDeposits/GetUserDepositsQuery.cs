using System;
using System.Collections.Generic;

using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

using FastEndpoints;

namespace DepositStopLoss.Application.Features.Deposits.Queries.GetUserDeposits;

/// <summary>
///     Query to get all deposits for a user.
/// </summary>
public sealed record GetUserDepositsQuery(Guid UserId) : ICommand<ErrorOr<IReadOnlyList<DepositDto>>>;

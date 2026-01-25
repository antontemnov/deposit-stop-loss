using System;
using System.Collections.Generic;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

namespace DepositStopLoss.Application.Features.Deposits.Queries.GetUserDeposits;

/// <summary>
///     Query to get all deposits for a user.
/// </summary>
public sealed record GetUserDepositsQuery(Guid UserId) : IQuery<ErrorOr<IReadOnlyList<DepositDto>>>;

using System;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

namespace DepositStopLoss.Application.Features.Users.Queries.GetUser;

/// <summary>
///     Query to get user by Id.
/// </summary>
public sealed record GetUserQuery(Guid UserId) : IQuery<ErrorOr<UserDto>>;

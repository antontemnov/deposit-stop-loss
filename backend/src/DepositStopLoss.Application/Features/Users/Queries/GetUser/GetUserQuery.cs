using System;

using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

using FastEndpoints;

namespace DepositStopLoss.Application.Features.Users.Queries.GetUser;

/// <summary>
///     Query to get user by Id.
/// </summary>
public sealed record GetUserQuery(Guid UserId) : ICommand<ErrorOr<UserDto>>;

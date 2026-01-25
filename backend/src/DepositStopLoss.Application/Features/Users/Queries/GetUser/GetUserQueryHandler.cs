using System;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

namespace DepositStopLoss.Application.Features.Users.Queries.GetUser;

/// <summary>
///     Handler for GetUserQuery.
/// </summary>
public sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, ErrorOr<UserDto>>
{
    public Task<ErrorOr<UserDto>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        // Stub: Find user by Id from repository, map to DTO, return
        throw new NotSupportedException("GetUserQueryHandler not implemented");
    }
}

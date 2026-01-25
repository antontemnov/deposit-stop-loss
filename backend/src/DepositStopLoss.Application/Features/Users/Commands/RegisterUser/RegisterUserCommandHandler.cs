using System;
using System.Threading;
using System.Threading.Tasks;

using DepositStopLoss.Application;
using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

namespace DepositStopLoss.Application.Features.Users.Commands.RegisterUser;

/// <summary>
///     Handler for RegisterUserCommand.
/// </summary>
public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, ErrorOr<UserDto>>
{
    public Task<ErrorOr<UserDto>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        // Stub: Check if user with TelegramId already exists, if exists return existing user, create new User aggregate, save to repository, return mapped DTO
        throw new NotSupportedException("RegisterUserCommandHandler not implemented");
    }
}

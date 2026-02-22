using DepositStopLoss.Application.Common.DTOs;

using ErrorOr;

using FastEndpoints;

namespace DepositStopLoss.Application.Features.Users.Commands.RegisterUser;

/// <summary>
///     Command to register a new user from Telegram.
/// </summary>
public sealed record RegisterUserCommand(long TelegramId, string? Username, string? FirstName, string? LastName, string LanguageCode)
    : ICommand<ErrorOr<UserDto>>;

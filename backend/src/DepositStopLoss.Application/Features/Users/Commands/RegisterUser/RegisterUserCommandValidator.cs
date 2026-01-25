using FluentValidation;

namespace DepositStopLoss.Application.Features.Users.Commands.RegisterUser;

/// <summary>
///     Validator for RegisterUserCommand.
/// </summary>
public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.TelegramId).GreaterThan(0).WithMessage("TelegramId must be a positive number");

        RuleFor(x => x.LanguageCode).NotEmpty().MaximumLength(10).WithMessage("LanguageCode is required and must be max 10 characters");

        RuleFor(x => x.Username).MaximumLength(100).When(x => x.Username is not null);

        RuleFor(x => x.FirstName).MaximumLength(100).When(x => x.FirstName is not null);

        RuleFor(x => x.LastName).MaximumLength(100).When(x => x.LastName is not null);
    }
}

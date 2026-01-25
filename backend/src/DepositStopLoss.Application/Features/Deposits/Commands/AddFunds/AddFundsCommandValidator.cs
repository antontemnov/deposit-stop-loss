using FluentValidation;

namespace DepositStopLoss.Application.Features.Deposits.Commands.AddFunds;

/// <summary>
///     Validator for AddFundsCommand.
/// </summary>
public sealed class AddFundsCommandValidator : AbstractValidator<AddFundsCommand>
{
    public AddFundsCommandValidator()
    {
        RuleFor(x => x.DepositId).NotEmpty().WithMessage("DepositId is required");

        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero");

        RuleFor(x => x.ExchangeRateAtContribution).GreaterThan(0).WithMessage("Exchange rate must be greater than zero");
    }
}

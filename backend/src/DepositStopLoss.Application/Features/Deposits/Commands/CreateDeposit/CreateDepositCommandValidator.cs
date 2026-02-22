using FluentValidation;

namespace DepositStopLoss.Application.Features.Deposits.Commands.CreateDeposit;

/// <summary>
///     Validator for CreateDepositCommand.
/// </summary>
public sealed class CreateDepositCommandValidator : AbstractValidator<CreateDepositCommand>
{
    public CreateDepositCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");

        RuleFor(x => x.BankId).NotEmpty().WithMessage("BankId is required");

        RuleFor(x => x.InitialAmount).GreaterThan(0).WithMessage("Initial amount must be greater than zero");

        RuleFor(x => x.Currency).NotEmpty().Length(3).WithMessage("Currency must be a 3-letter code (e.g., GEL, USD)");

        RuleFor(x => x.AnnualInterestRate).InclusiveBetween(0, 100).WithMessage("Annual interest rate must be between 0 and 100");

        RuleFor(x => x.TermMonths).InclusiveBetween(1, 120).WithMessage("Term must be between 1 and 120 months");

        RuleFor(x => x.RateType)
            .NotEmpty()
            .Must(x => x is "Commercial" or "Discounted")
            .WithMessage("Rate type must be 'Commercial' or 'Discounted'");

        RuleFor(x => x.StopLossThreshold).InclusiveBetween(-50, 0).WithMessage("Stop loss threshold must be between -50% and 0%");
    }
}

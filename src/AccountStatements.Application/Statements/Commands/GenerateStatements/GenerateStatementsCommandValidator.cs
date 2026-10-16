using FluentValidation;

namespace AccountStatements.Application.Statements.Commands.GenerateStatements;

public class GenerateStatementsCommandValidator : AbstractValidator<GenerateStatementsCommand>
{
    public GenerateStatementsCommandValidator()
    {
        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100).WithMessage("Year must be between 2000 and 2100.");

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12).WithMessage("Month must be between 1 and 12.");

        RuleFor(x => x.Month)
            .Must((cmd, _) =>
            {
                var now = DateTime.UtcNow;
                return cmd.Year * 100 + cmd.Month <= now.Year * 100 + now.Month;
            })
            .WithMessage("Statements cannot be generated for a future month.");
    }
}

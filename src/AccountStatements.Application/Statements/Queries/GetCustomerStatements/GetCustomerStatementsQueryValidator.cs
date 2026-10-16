using FluentValidation;

namespace AccountStatements.Application.Statements.Queries.GetCustomerStatements;

public class GetCustomerStatementsQueryValidator : AbstractValidator<GetCustomerStatementsQuery>
{
    public GetCustomerStatementsQueryValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x)
            .Must(x => x.FromYear.HasValue == x.FromMonth.HasValue)
            .WithMessage("FromYear and FromMonth must be provided together.");

        RuleFor(x => x)
            .Must(x => x.ToYear.HasValue == x.ToMonth.HasValue)
            .WithMessage("ToYear and ToMonth must be provided together.");

        RuleFor(x => x.FromYear!.Value)
            .InclusiveBetween(2000, 2100).WithMessage("FromYear must be between 2000 and 2100.")
            .When(x => x.FromYear.HasValue);

        RuleFor(x => x.FromMonth!.Value)
            .InclusiveBetween(1, 12).WithMessage("FromMonth must be between 1 and 12.")
            .When(x => x.FromMonth.HasValue);

        RuleFor(x => x.ToYear!.Value)
            .InclusiveBetween(2000, 2100).WithMessage("ToYear must be between 2000 and 2100.")
            .When(x => x.ToYear.HasValue);

        RuleFor(x => x.ToMonth!.Value)
            .InclusiveBetween(1, 12).WithMessage("ToMonth must be between 1 and 12.")
            .When(x => x.ToMonth.HasValue);

        RuleFor(x => x)
            .Must(x => x.FromYear!.Value * 100 + x.FromMonth!.Value <= x.ToYear!.Value * 100 + x.ToMonth!.Value)
            .WithMessage("The from month must not be after the to month.")
            .When(x => x.FromYear.HasValue && x.FromMonth.HasValue && x.ToYear.HasValue && x.ToMonth.HasValue);
    }
}

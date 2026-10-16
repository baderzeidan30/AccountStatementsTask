using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace AccountStatements.Api.Swagger;

public class ValidationErrorExample : IExamplesProvider<ValidationProblemDetails>
{
    public ValidationProblemDetails GetExamples() =>
        new(new Dictionary<string, string[]>
        {
            ["Month"] = new[] { "Month must be between 1 and 12." }
        })
        {
            Status = 400,
            Title = "Validation failed"
        };
}

public class NotFoundErrorExample : IExamplesProvider<ProblemDetails>
{
    public ProblemDetails GetExamples() => new()
    {
        Status = 404,
        Title = "Not found",
        Detail = "No statement for customer 3f2b8c1e-7a4d-4e6b-9c1a-5d8e2f4b6a10 for 2026-09."
    };
}

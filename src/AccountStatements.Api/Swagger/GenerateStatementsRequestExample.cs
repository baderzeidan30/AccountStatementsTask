using AccountStatements.Api.Contracts;
using Swashbuckle.AspNetCore.Filters;

namespace AccountStatements.Api.Swagger;

public class GenerateStatementsRequestExample : IExamplesProvider<GenerateStatementsRequest>
{
    public GenerateStatementsRequest GetExamples() => new(2026, 9);
}

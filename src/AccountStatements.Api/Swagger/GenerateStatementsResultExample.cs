using AccountStatements.Application.Statements.Commands.GenerateStatements;
using Swashbuckle.AspNetCore.Filters;

namespace AccountStatements.Api.Swagger;

public class GenerateStatementsResultExample : IExamplesProvider<GenerateStatementsResult>
{
    public GenerateStatementsResult GetExamples() => new(
        TotalCustomers: 3,
        Generated: 2,
        Skipped: 1,
        EmailsSent: 2,
        EmailsFailed: 0);
}

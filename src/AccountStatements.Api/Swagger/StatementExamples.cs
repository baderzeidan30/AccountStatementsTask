using AccountStatements.Application.Statements;
using Swashbuckle.AspNetCore.Filters;

namespace AccountStatements.Api.Swagger;

public static class StatementSamples
{
    public static readonly Guid CustomerId = Guid.Parse("3f2b8c1e-7a4d-4e6b-9c1a-5d8e2f4b6a10");

    public static StatementDto September => new(
        Guid.Parse("a1c4e7b2-0d3f-4b8a-9e51-6c2d7f8a1b34"),
        CustomerId,
        2026,
        9,
        OpeningBalance: 1500.00m,
        TotalCredits: 2500.00m,
        TotalDebits: 900.50m,
        ClosingBalance: 3099.50m,
        GeneratedAtUtc: new DateTime(2026, 10, 1, 6, 0, 0, DateTimeKind.Utc),
        EmailedAtUtc: new DateTime(2026, 10, 1, 6, 0, 3, DateTimeKind.Utc),
        EmailStatus: "Sent");

    public static StatementDto August => new(
        Guid.Parse("b2d5f8c3-1e4a-4c9b-8f62-7d3e8a9b2c45"),
        CustomerId,
        2026,
        8,
        OpeningBalance: 1000.00m,
        TotalCredits: 1200.00m,
        TotalDebits: 700.00m,
        ClosingBalance: 1500.00m,
        GeneratedAtUtc: new DateTime(2026, 9, 1, 6, 0, 0, DateTimeKind.Utc),
        EmailedAtUtc: new DateTime(2026, 9, 1, 6, 0, 2, DateTimeKind.Utc),
        EmailStatus: "Sent");
}

public class StatementDtoExample : IExamplesProvider<StatementDto>
{
    public StatementDto GetExamples() => StatementSamples.September;
}

public class StatementDtoListExample : IExamplesProvider<List<StatementDto>>
{
    public List<StatementDto> GetExamples() => new() { StatementSamples.September, StatementSamples.August };
}

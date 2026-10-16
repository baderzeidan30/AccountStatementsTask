using AccountStatements.Domain.Entities;

namespace AccountStatements.Application.Statements;

public static class StatementMappings
{
    public static StatementDto ToDto(this Statement s) => new(
        s.Id,
        s.CustomerId,
        s.Year,
        s.Month,
        s.OpeningBalance,
        s.TotalCredits,
        s.TotalDebits,
        s.ClosingBalance,
        s.GeneratedAtUtc,
        s.EmailedAtUtc,
        s.EmailStatus.ToString());
}

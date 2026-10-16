namespace AccountStatements.Application.Statements;

public record StatementDto(
    Guid Id,
    Guid CustomerId,
    int Year,
    int Month,
    decimal OpeningBalance,
    decimal TotalCredits,
    decimal TotalDebits,
    decimal ClosingBalance,
    DateTime GeneratedAtUtc,
    DateTime? EmailedAtUtc,
    string EmailStatus);

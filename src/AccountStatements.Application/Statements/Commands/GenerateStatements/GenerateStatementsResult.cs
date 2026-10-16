namespace AccountStatements.Application.Statements.Commands.GenerateStatements;

public record GenerateStatementsResult(
    int TotalCustomers,
    int Generated,
    int Skipped,
    int EmailsSent,
    int EmailsFailed);

using MediatR;

namespace AccountStatements.Application.Statements.Commands.GenerateStatements;

public record GenerateStatementsCommand(int Year, int Month) : IRequest<GenerateStatementsResult>;

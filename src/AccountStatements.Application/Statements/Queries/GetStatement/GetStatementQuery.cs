using MediatR;

namespace AccountStatements.Application.Statements.Queries.GetStatement;

public record GetStatementQuery(Guid CustomerId, int Year, int Month) : IRequest<StatementDto>;

using MediatR;

namespace AccountStatements.Application.Statements.Queries.GetCustomerStatements;

public record GetCustomerStatementsQuery(
    Guid CustomerId,
    int? FromYear,
    int? FromMonth,
    int? ToYear,
    int? ToMonth) : IRequest<List<StatementDto>>;

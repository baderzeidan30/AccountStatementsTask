using AccountStatements.Application.Abstractions;
using AccountStatements.Application.Common.Exceptions;
using MediatR;

namespace AccountStatements.Application.Statements.Queries.GetCustomerStatements;

public class GetCustomerStatementsQueryHandler : IRequestHandler<GetCustomerStatementsQuery, List<StatementDto>>
{
    private readonly ICustomerRepository _customers;
    private readonly IStatementRepository _statements;

    public GetCustomerStatementsQueryHandler(ICustomerRepository customers, IStatementRepository statements)
    {
        _customers = customers;
        _statements = statements;
    }

    public async Task<List<StatementDto>> Handle(GetCustomerStatementsQuery request, CancellationToken ct)
    {
        var customer = await _customers.GetByIdAsync(request.CustomerId, ct);
        if (customer is null)
            throw new NotFoundException($"Customer {request.CustomerId} was not found.");

        // Encode year + month as one number, e.g. 2026-09 becomes 202609
        int? fromPeriod = request.FromYear.HasValue && request.FromMonth.HasValue
            ? request.FromYear.Value * 100 + request.FromMonth.Value
            : null;

        int? toPeriod = request.ToYear.HasValue && request.ToMonth.HasValue
            ? request.ToYear.Value * 100 + request.ToMonth.Value
            : null;

        var statements = await _statements.GetByCustomerAsync(request.CustomerId, fromPeriod, toPeriod, ct);

        return statements.Select(s => s.ToDto()).ToList();
    }
}

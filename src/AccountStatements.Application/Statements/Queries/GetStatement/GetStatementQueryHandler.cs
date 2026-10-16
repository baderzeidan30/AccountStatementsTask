using AccountStatements.Application.Abstractions;
using AccountStatements.Application.Common.Exceptions;
using MediatR;

namespace AccountStatements.Application.Statements.Queries.GetStatement;

public class GetStatementQueryHandler : IRequestHandler<GetStatementQuery, StatementDto>
{
    private readonly ICustomerRepository _customers;
    private readonly IStatementRepository _statements;

    public GetStatementQueryHandler(ICustomerRepository customers, IStatementRepository statements)
    {
        _customers = customers;
        _statements = statements;
    }

    public async Task<StatementDto> Handle(GetStatementQuery request, CancellationToken ct)
    {
        var customer = await _customers.GetByIdAsync(request.CustomerId, ct);
        if (customer is null)
            throw new NotFoundException($"Customer {request.CustomerId} was not found.");

        var statement = await _statements.GetAsync(request.CustomerId, request.Year, request.Month, ct);
        if (statement is null)
            throw new NotFoundException($"No statement for customer {request.CustomerId} for {request.Year}-{request.Month:D2}.");

        return statement.ToDto();
    }
}

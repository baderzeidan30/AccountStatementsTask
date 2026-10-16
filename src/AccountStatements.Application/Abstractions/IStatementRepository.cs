using AccountStatements.Domain.Entities;

namespace AccountStatements.Application.Abstractions;

public interface IStatementRepository
{
    Task<bool> ExistsAsync(Guid customerId, int year, int month, CancellationToken ct);

    Task<Statement?> GetAsync(Guid customerId, int year, int month, CancellationToken ct);

    // Same as GetAsync but tracked, so changes (e.g. email status) can be saved
    Task<Statement?> GetForUpdateAsync(Guid customerId, int year, int month, CancellationToken ct);

    // Statements for a customer between two months (inclusive), newest first.
    // fromPeriod / toPeriod are encoded as year * 100 + month (e.g. 202609).
    Task<List<Statement>> GetByCustomerAsync(
        Guid customerId, int? fromPeriod, int? toPeriod, CancellationToken ct);

    Task AddAsync(Statement statement, CancellationToken ct);
}

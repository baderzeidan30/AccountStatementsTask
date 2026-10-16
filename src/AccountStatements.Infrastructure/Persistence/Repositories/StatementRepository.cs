using AccountStatements.Application.Abstractions;
using AccountStatements.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountStatements.Infrastructure.Persistence.Repositories;

public class StatementRepository : IStatementRepository
{
    private readonly AppDbContext _db;

    public StatementRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<bool> ExistsAsync(Guid customerId, int year, int month, CancellationToken ct) =>
        _db.Statements.AnyAsync(s => s.CustomerId == customerId && s.Year == year && s.Month == month, ct);

    public Task<Statement?> GetAsync(Guid customerId, int year, int month, CancellationToken ct) =>
        _db.Statements.AsNoTracking()
            .FirstOrDefaultAsync(s => s.CustomerId == customerId && s.Year == year && s.Month == month, ct);

    public Task<List<Statement>> GetByCustomerAsync(
        Guid customerId, int? fromPeriod, int? toPeriod, CancellationToken ct)
    {
        var query = _db.Statements.AsNoTracking().Where(s => s.CustomerId == customerId);

        if (fromPeriod.HasValue)
            query = query.Where(s => s.Year * 100 + s.Month >= fromPeriod.Value);

        if (toPeriod.HasValue)
            query = query.Where(s => s.Year * 100 + s.Month <= toPeriod.Value);

        return query
            .OrderByDescending(s => s.Year)
            .ThenByDescending(s => s.Month)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Statement statement, CancellationToken ct) =>
        await _db.Statements.AddAsync(statement, ct);
}

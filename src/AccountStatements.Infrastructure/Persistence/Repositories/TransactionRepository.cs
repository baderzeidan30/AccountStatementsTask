using AccountStatements.Application.Abstractions;
using AccountStatements.Domain.Entities;
using AccountStatements.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AccountStatements.Infrastructure.Persistence.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _db;

    public TransactionRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Transaction>> GetByPeriodAsync(
        Guid customerId, DateTime fromInclusive, DateTime toExclusive, CancellationToken ct) =>
        _db.Transactions
            .AsNoTracking()
            .Where(t => t.CustomerId == customerId && t.Date >= fromInclusive && t.Date < toExclusive)
            .OrderBy(t => t.Date)
            .ToListAsync(ct);

    public async Task<decimal> GetBalanceBeforeAsync(Guid customerId, DateTime date, CancellationToken ct)
    {
        // SQLite cannot sum decimals in SQL, so we sum in memory
        var rows = await _db.Transactions
            .AsNoTracking()
            .Where(t => t.CustomerId == customerId && t.Date < date)
            .Select(t => new { t.Type, t.Amount })
            .ToListAsync(ct);

        return rows.Sum(r => r.Type == TransactionType.Credit ? r.Amount : -r.Amount);
    }
}

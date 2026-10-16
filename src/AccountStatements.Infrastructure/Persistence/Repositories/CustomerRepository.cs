using AccountStatements.Application.Abstractions;
using AccountStatements.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountStatements.Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;

    public CustomerRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<List<Customer>> GetAllAsync(CancellationToken ct) =>
        _db.Customers.AsNoTracking().OrderBy(c => c.FullName).ToListAsync(ct);

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _db.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);
}

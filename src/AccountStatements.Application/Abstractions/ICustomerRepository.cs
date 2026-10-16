using AccountStatements.Domain.Entities;

namespace AccountStatements.Application.Abstractions;

public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync(CancellationToken ct);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken ct);
}

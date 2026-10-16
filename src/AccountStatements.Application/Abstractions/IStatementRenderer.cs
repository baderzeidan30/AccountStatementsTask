using AccountStatements.Domain.Entities;

namespace AccountStatements.Application.Abstractions;

public interface IStatementRenderer
{
    string Render(Customer customer, Statement statement, IReadOnlyList<Transaction> transactions);
}

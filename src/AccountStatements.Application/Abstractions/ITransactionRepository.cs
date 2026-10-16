using AccountStatements.Domain.Entities;

namespace AccountStatements.Application.Abstractions;

public interface ITransactionRepository
{
    // Transactions inside [fromInclusive, toExclusive)
    Task<List<Transaction>> GetByPeriodAsync(
        Guid customerId, DateTime fromInclusive, DateTime toExclusive, CancellationToken ct);

    // Net balance (credits - debits) of everything before the given date.
    // Used as the statement opening balance.
    Task<decimal> GetBalanceBeforeAsync(Guid customerId, DateTime date, CancellationToken ct);
}

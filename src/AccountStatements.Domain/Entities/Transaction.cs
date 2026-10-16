using AccountStatements.Domain.Enums;

namespace AccountStatements.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = default!;
    public decimal Amount { get; set; }        // always positive; Type says the direction
    public TransactionType Type { get; set; }
}

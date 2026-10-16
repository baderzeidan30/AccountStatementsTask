using AccountStatements.Domain.Enums;

namespace AccountStatements.Domain.Entities;

public class Statement
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }

    public decimal OpeningBalance { get; set; }
    public decimal TotalCredits { get; set; }
    public decimal TotalDebits { get; set; }
    public decimal ClosingBalance => OpeningBalance + TotalCredits - TotalDebits;

    public string Body { get; set; } = default!;   // the HTML that was emailed
    public DateTime GeneratedAtUtc { get; set; }
    public DateTime? EmailedAtUtc { get; set; }
    public EmailStatus EmailStatus { get; set; }
}

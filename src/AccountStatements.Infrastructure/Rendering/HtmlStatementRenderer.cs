using System.Globalization;
using System.Net;
using System.Text;
using AccountStatements.Application.Abstractions;
using AccountStatements.Domain.Entities;
using AccountStatements.Domain.Enums;

namespace AccountStatements.Infrastructure.Rendering;

public class HtmlStatementRenderer : IStatementRenderer
{
    public string Render(Customer customer, Statement statement, IReadOnlyList<Transaction> transactions)
    {
        var monthName = new DateTime(statement.Year, statement.Month, 1)
            .ToString("MMMM yyyy", CultureInfo.InvariantCulture);

        var sb = new StringBuilder();
        sb.AppendLine("<html><body style='font-family:Arial,sans-serif'>");
        sb.AppendLine($"<h2>Account Statement - {monthName}</h2>");
        sb.AppendLine($"<p>Dear {WebUtility.HtmlEncode(customer.FullName)},</p>");
        sb.AppendLine("<p>Please find your account statement below.</p>");

        sb.AppendLine("<table style='border-collapse:collapse;width:100%' border='1' cellpadding='6'>");
        sb.AppendLine("<tr><th>Date</th><th>Description</th><th>Type</th><th>Amount</th></tr>");

        foreach (var t in transactions)
        {
            var type = t.Type == TransactionType.Credit ? "Credit" : "Debit";
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td>{t.Date:yyyy-MM-dd}</td>");
            sb.AppendLine($"<td>{WebUtility.HtmlEncode(t.Description)}</td>");
            sb.AppendLine($"<td>{type}</td>");
            sb.AppendLine($"<td style='text-align:right'>{Money(t.Amount)}</td>");
            sb.AppendLine("</tr>");
        }

        if (transactions.Count == 0)
            sb.AppendLine("<tr><td colspan='4'>No transactions this month.</td></tr>");

        sb.AppendLine("</table>");

        sb.AppendLine("<h3>Summary</h3>");
        sb.AppendLine("<table cellpadding='4'>");
        sb.AppendLine($"<tr><td>Opening balance</td><td style='text-align:right'>{Money(statement.OpeningBalance)}</td></tr>");
        sb.AppendLine($"<tr><td>Total credits</td><td style='text-align:right'>{Money(statement.TotalCredits)}</td></tr>");
        sb.AppendLine($"<tr><td>Total debits</td><td style='text-align:right'>{Money(statement.TotalDebits)}</td></tr>");
        sb.AppendLine($"<tr><td><b>Closing balance</b></td><td style='text-align:right'><b>{Money(statement.ClosingBalance)}</b></td></tr>");
        sb.AppendLine("</table>");

        sb.AppendLine("</body></html>");
        return sb.ToString();
    }

    private static string Money(decimal value) => value.ToString("N2", CultureInfo.InvariantCulture);
}

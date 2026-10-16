using AccountStatements.Application.Abstractions;
using AccountStatements.Domain.Entities;
using AccountStatements.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccountStatements.Application.Statements.Commands.GenerateStatements;

public class GenerateStatementsCommandHandler : IRequestHandler<GenerateStatementsCommand, GenerateStatementsResult>
{
    private readonly ICustomerRepository _customers;
    private readonly ITransactionRepository _transactions;
    private readonly IStatementRepository _statements;
    private readonly IStatementRenderer _renderer;
    private readonly IEmailSender _emailSender;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GenerateStatementsCommandHandler> _logger;

    public GenerateStatementsCommandHandler(
        ICustomerRepository customers,
        ITransactionRepository transactions,
        IStatementRepository statements,
        IStatementRenderer renderer,
        IEmailSender emailSender,
        IUnitOfWork unitOfWork,
        ILogger<GenerateStatementsCommandHandler> logger)
    {
        _customers = customers;
        _transactions = transactions;
        _statements = statements;
        _renderer = renderer;
        _emailSender = emailSender;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<GenerateStatementsResult> Handle(GenerateStatementsCommand request, CancellationToken ct)
    {
        var periodStart = new DateTime(request.Year, request.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var periodEnd = periodStart.AddMonths(1);

        var customers = await _customers.GetAllAsync(ct);
        int generated = 0, skipped = 0, sent = 0, failed = 0;

        foreach (var customer in customers)
        {
            // Idempotency: one statement per customer per month
            if (await _statements.ExistsAsync(customer.Id, request.Year, request.Month, ct))
            {
                skipped++;
                continue;
            }

            var transactions = await _transactions.GetByPeriodAsync(customer.Id, periodStart, periodEnd, ct);
            var openingBalance = await _transactions.GetBalanceBeforeAsync(customer.Id, periodStart, ct);

            var statement = new Statement
            {
                Id = Guid.NewGuid(),
                CustomerId = customer.Id,
                Year = request.Year,
                Month = request.Month,
                OpeningBalance = openingBalance,
                TotalCredits = transactions.Where(t => t.Type == TransactionType.Credit).Sum(t => t.Amount),
                TotalDebits = transactions.Where(t => t.Type == TransactionType.Debit).Sum(t => t.Amount),
                GeneratedAtUtc = DateTime.UtcNow,
                EmailStatus = EmailStatus.Pending
            };

            statement.Body = _renderer.Render(customer, statement, transactions);

            // Save first, so the statement exists even if the email fails
            await _statements.AddAsync(statement, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            generated++;

            try
            {
                var subject = $"Your account statement for {request.Year}-{request.Month:D2}";
                await _emailSender.SendAsync(customer.Email, subject, statement.Body, ct);

                statement.EmailStatus = EmailStatus.Sent;
                statement.EmailedAtUtc = DateTime.UtcNow;
                sent++;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // One failed email must not stop the other customers
                _logger.LogError(ex, "Failed to email statement for customer {CustomerId}", customer.Id);
                statement.EmailStatus = EmailStatus.Failed;
                failed++;
            }

            await _unitOfWork.SaveChangesAsync(ct);
        }

        return new GenerateStatementsResult(customers.Count, generated, skipped, sent, failed);
    }
}

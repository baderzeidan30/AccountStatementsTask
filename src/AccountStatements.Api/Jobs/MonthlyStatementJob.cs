using AccountStatements.Application.Statements.Commands.GenerateStatements;
using MediatR;

namespace AccountStatements.Api.Jobs;

public class MonthlyStatementJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MonthlyStatementJob> _logger;

    public MonthlyStatementJob(
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<MonthlyStatementJob> logger)
    {
        _scopeFactory = scopeFactory;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_configuration.GetValue("Scheduler:Enabled", true))
        {
            _logger.LogInformation("Monthly statement job is disabled.");
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextRun = GetNextRun(now);
            _logger.LogInformation("Next statement run at {NextRun:u}", nextRun);

            try
            {
                await Task.Delay(nextRun - now, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            await RunAsync(stoppingToken);
        }
    }

    // 06:00 UTC on the 1st of a month: this month if still ahead, otherwise next month
    public static DateTime GetNextRun(DateTime nowUtc)
    {
        var thisMonthRun = new DateTime(nowUtc.Year, nowUtc.Month, 1, 6, 0, 0, DateTimeKind.Utc);
        return nowUtc < thisMonthRun ? thisMonthRun : thisMonthRun.AddMonths(1);
    }

    private async Task RunAsync(CancellationToken ct)
    {
        var previousMonth = DateTime.UtcNow.AddMonths(-1);

        try
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var result = await mediator.Send(
                new GenerateStatementsCommand(previousMonth.Year, previousMonth.Month), ct);

            _logger.LogInformation(
                "Statements {Year}-{Month:D2}: generated {Generated}, skipped {Skipped}, sent {Sent}, failed {Failed}",
                previousMonth.Year, previousMonth.Month,
                result.Generated, result.Skipped, result.EmailsSent, result.EmailsFailed);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            // A failed run must not kill the service; the next month will try again
            _logger.LogError(ex, "Monthly statement run failed");
        }
    }
}

# Account Statements API

A small Web API that generates monthly account statements, emails them to customers, and lets you fetch statements by customer ID and month.

Built with .NET 9, Clean Architecture, CQRS, MediatR, FluentValidation, EF Core (SQLite), MailKit and Swagger.

## Run

    dotnet run --project src\AccountStatements.Api

Open http://localhost:5170/swagger. The database is created and seeded with 3 customers on first run.

## Endpoints

- POST /api/statements/generate : generate and email statements for a month. Body: { "year": 2026, "month": 9 }
- GET /api/customers/{customerId}/statements : list statements, optional fromYear, fromMonth, toYear, toMonth
- GET /api/customers/{customerId}/statements/{year}/{month} : get one statement

Test customer ID: 3f2b8c1e-7a4d-4e6b-9c1a-5d8e2f4b6a10

## Layers

- Domain: entities and enums
- Application: commands, queries, validators, interfaces
- Infrastructure: database, repositories, SMTP email, HTML renderer
- Api: controllers, error handling, Swagger, monthly job

## Email

SMTP settings are in the Email section of appsettings.json. A monthly background job also sends the previous month statements on the 1st of each month at 06:00 UTC.

## Notes

- One statement per customer per month, so running the job twice creates no duplicates.
- If an email fails, the statement is saved as Failed and retried on the next run.
- Swashbuckle 6.9.0 is paired with Microsoft.OpenApi 1.6.22 so Swagger UI renders correctly.

## Monthly job settings

The monthly job is controlled from the Scheduler section of appsettings.json:

- Enabled (default true): turns the monthly job on or off.
- RunOnStartup (default false): when true, the job runs once as soon as the app starts, for the previous month. Use it to test sending real emails without waiting for the 1st of the month. Set it back to false afterwards.

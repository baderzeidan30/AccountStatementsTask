using AccountStatements.Domain.Entities;
using AccountStatements.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AccountStatements.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        // Seed only once
        if (await db.Customers.AnyAsync())
            return;

        var ahmad = new Customer
        {
            Id = Guid.Parse("3f2b8c1e-7a4d-4e6b-9c1a-5d8e2f4b6a10"),
            FullName = "Ahmad Nasser",
            Email = "baderzeidan30@gmail.com"
        };
        var layla = new Customer
        {
            Id = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
            FullName = "Layla Haddad",
            Email = "layla.haddad@example.com"
        };
        var omar = new Customer
        {
            Id = Guid.Parse("c56a4180-65aa-42ec-a945-5fd21dec0538"),
            FullName = "Omar Khalil",
            Email = "omar.khalil@example.com"
        };

        db.Customers.AddRange(ahmad, layla, omar);

        db.Transactions.AddRange(
            // Ahmad: opening balance for September = 1000 + 1200 - 700 = 1500
            Tx(ahmad.Id, 2026, 7, 5, "Opening deposit", 1000.00m, TransactionType.Credit),
            Tx(ahmad.Id, 2026, 8, 3, "Salary", 1200.00m, TransactionType.Credit),
            Tx(ahmad.Id, 2026, 8, 12, "Rent", 700.00m, TransactionType.Debit),
            // Ahmad: September credits 2500.00, debits 900.50
            Tx(ahmad.Id, 2026, 9, 3, "Salary", 2500.00m, TransactionType.Credit),
            Tx(ahmad.Id, 2026, 9, 8, "Groceries", 300.25m, TransactionType.Debit),
            Tx(ahmad.Id, 2026, 9, 15, "Electricity bill", 150.25m, TransactionType.Debit),
            Tx(ahmad.Id, 2026, 9, 22, "Online shopping", 450.00m, TransactionType.Debit),

            // Layla
            Tx(layla.Id, 2026, 8, 1, "Opening deposit", 5000.00m, TransactionType.Credit),
            Tx(layla.Id, 2026, 9, 10, "Transfer to savings", 800.00m, TransactionType.Debit),
            Tx(layla.Id, 2026, 9, 20, "Interest", 12.50m, TransactionType.Credit),

            // Omar
            Tx(omar.Id, 2026, 9, 2, "Opening deposit", 300.00m, TransactionType.Credit),
            Tx(omar.Id, 2026, 9, 15, "Phone bill", 45.00m, TransactionType.Debit));

        await db.SaveChangesAsync();
    }

    private static Transaction Tx(
        Guid customerId, int year, int month, int day,
        string description, decimal amount, TransactionType type) => new()
    {
        Id = Guid.NewGuid(),
        CustomerId = customerId,
        Date = new DateTime(year, month, day, 10, 0, 0, DateTimeKind.Utc),
        Description = description,
        Amount = amount,
        Type = type
    };
}




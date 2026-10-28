using AccountStatements.Domain.Enums;

namespace AccountStatements.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;

    public CustomerType CustomerType { get;set;}
}

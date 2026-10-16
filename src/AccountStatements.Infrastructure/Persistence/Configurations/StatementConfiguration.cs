using AccountStatements.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountStatements.Infrastructure.Persistence.Configurations;

public class StatementConfiguration : IEntityTypeConfiguration<Statement>
{
    public void Configure(EntityTypeBuilder<Statement> builder)
    {
        builder.HasKey(s => s.Id);

        // Computed in the entity, never stored
        builder.Ignore(s => s.ClosingBalance);

        builder.Property(s => s.OpeningBalance).HasPrecision(18, 2);
        builder.Property(s => s.TotalCredits).HasPrecision(18, 2);
        builder.Property(s => s.TotalDebits).HasPrecision(18, 2);
        builder.Property(s => s.Body).IsRequired();

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(s => s.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => new { s.CustomerId, s.Year, s.Month }).IsUnique();
    }
}

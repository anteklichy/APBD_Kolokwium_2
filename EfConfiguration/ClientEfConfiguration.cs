namespace Kolokwium2.EfConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class ClientEfConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(e => e.IdClient);
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
        builder.Property(e => e.Phone).HasMaxLength(100);

        builder.HasMany(e => e.Sales)
            .WithOne(s => s.Client)
            .HasForeignKey(s => s.IdClient);

        builder.HasMany(e => e.Payments)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.IdClient);

        builder.HasData(
            new Client { IdClient = 1, FirstName = "Jan", LastName = "Kowalski", Email = "kowalski@wp.pl", Phone = "444-333-231" },
            new Client { IdClient = 2, FirstName = "Anna", LastName = "Nowak", Email = "nowak@wp.pl", Phone = "555-333-231" }
        );
    }
}
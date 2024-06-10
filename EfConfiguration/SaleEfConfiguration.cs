namespace Kolokwium2.EfConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class SaleEfConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.HasKey(e => e.IdSale);
        builder.Property(e => e.CreatedAt).IsRequired();

        builder.HasOne(e => e.Client)
            .WithMany(c => c.Sales)
            .HasForeignKey(e => e.IdClient);

        builder.HasOne(e => e.Subscription)
            .WithMany(s => s.Sales)
            .HasForeignKey(e => e.IdSubscription);

        builder.HasData(
            new Sale { IdSale = 1, IdClient = 1, IdSubscription = 1, CreatedAt = new DateTime(2024, 1, 1) },
            new Sale { IdSale = 2, IdClient = 2, IdSubscription = 2, CreatedAt = new DateTime(2024, 1, 1) }
        );
    }
}

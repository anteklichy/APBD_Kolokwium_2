namespace Kolokwium2.EfConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class SubscriptionEfConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(e => e.IdSubscription);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
        builder.Property(e => e.RenewalPeriod).IsRequired();
        builder.Property(e => e.EndTime).IsRequired();
        builder.Property(e => e.Price).IsRequired().HasColumnType("money");

        builder.HasMany(e => e.Sales)
            .WithOne(s => s.Subscription)
            .HasForeignKey(s => s.IdSubscription);

        builder.HasMany(e => e.Payments)
            .WithOne(p => p.Subscription)
            .HasForeignKey(p => p.IdSubscription);

        builder.HasData(
            new Subscription { IdSubscription = 1, Name = "Basic", RenewalPeriod = 1, EndTime = new DateTime(2025, 1, 1), Price = 100 },
            new Subscription { IdSubscription = 2, Name = "Premium", RenewalPeriod = 3, EndTime = new DateTime(2025, 1, 1), Price = 250 }
        );
    }
}

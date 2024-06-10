namespace Kolokwium2.EfConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class DiscountEfConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(e => e.IdDiscount);
        builder.Property(e => e.Value).IsRequired();
        builder.Property(e => e.DateFrom).IsRequired();
        builder.Property(e => e.DateTo).IsRequired();

        builder.HasOne(e => e.Subscription)
            .WithMany(s => s.Discounts)
            .HasForeignKey(e => e.IdSubscription);
    }
}
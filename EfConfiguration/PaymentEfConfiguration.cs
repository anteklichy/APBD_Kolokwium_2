namespace Kolokwium2.EfConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models;

public class PaymentEfConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(e => e.IdPayment);
        builder.Property(e => e.Date).IsRequired();

        builder.HasOne(e => e.Client)
            .WithMany(c => c.Payments)
            .HasForeignKey(e => e.IdClient);

        builder.HasOne(e => e.Subscription)
            .WithMany(s => s.Payments)
            .HasForeignKey(e => e.IdSubscription);
    }
}
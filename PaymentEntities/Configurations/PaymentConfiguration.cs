using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentEntities.Entities;

namespace PaymentEntities.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder
                .Property(s => s.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .Property(s => s.CreditCardNumber)
                .IsRequired();

            builder
                .Property(s => s.CardHolder)
                .IsRequired();

            builder
                .Property(s => s.Amount)
                .IsRequired();

            builder
                .Property(s => s.SecurityCode)
                .HasMaxLength(3);

            builder
                .HasOne<PaymentStatus>(s => s.PaymentStatus)
                .WithOne(g => g.Payment)
                .HasForeignKey<PaymentStatus>(s => s.PaymentId);
        }
    }
}

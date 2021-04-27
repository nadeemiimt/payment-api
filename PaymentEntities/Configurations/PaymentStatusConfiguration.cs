using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentEntities.Entities;

namespace PaymentEntities.Configurations
{
    public class PaymentStatusConfiguration : IEntityTypeConfiguration<PaymentStatus>
    {
        public void Configure(EntityTypeBuilder<PaymentStatus> builder)
        {
            builder
                .Property(s => s.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder
                .Property(s => s.PaymentId)
                .IsRequired();

            builder
                .Property(s => s.StatusId)
                .IsRequired();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentEntities.Entities;

namespace PaymentEntities.Configurations
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder
                .Property(s => s.Name)
                .IsRequired();

            builder
                .HasOne<PaymentStatus>(s => s.PaymentStatus)
                .WithOne(g => g.Status)
                .HasForeignKey<PaymentStatus>(s => s.StatusId);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PaymentEntities.Entities;

namespace PaymentEntities.Helper
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Seed data.
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>().HasData(
                new Status
                {
                    Id = 1,
                    Name = "Pending"
                },
                new Status
                {
                    Id = 2,
                    Name = "Processed"
                },
                new Status
                {
                    Id = 3,
                    Name = "Failed"
                }
            );
        }
    }
}

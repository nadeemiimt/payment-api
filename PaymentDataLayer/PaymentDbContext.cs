using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using PaymentEntities.Configurations;
using PaymentEntities.Entities;
using PaymentEntities.Helper;

namespace PaymentDataLayer
{
    /* Migration commands:
     From npm:
         1: add-migration initial
         2: update-database
     OR From root project location in cmd
     dotnet ef --startup-project PaymentApi/PaymentApi.csproj migrations add InitialModel -p PaymentDataLayer/PaymentDataLayer.csproj
     dotnet ef --startup-project PaymentApi/PaymentApi.csproj database update */

    /// <summary>
    /// Core db context.
    /// </summary>
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        { }

        /// <summary>
        /// Override model creation event for seed and configurations.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration
            modelBuilder
                .ApplyConfiguration(new StatusConfiguration());
            modelBuilder
                .ApplyConfiguration(new PaymentConfiguration());
            modelBuilder
                .ApplyConfiguration(new PaymentStatusConfiguration());

            // Seed
            modelBuilder.Seed();
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<PaymentStatus> PaymentsStatus { get; set; }
    }
}

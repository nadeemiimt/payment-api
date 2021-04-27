using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentBusiness.Payment.Gateway;
using PaymentCommon.Interfaces;
using PaymentCommon.Resources;
using PaymentDataLayer;
using PaymentEntities.Interfaces;

namespace PaymentBusiness.DI
{
    /// <summary>
    /// Register dependencies which are not accessible in API layer.
    /// </summary>
    public static class Register
    {
        /// <summary>
        /// Resolver delegate to resolve Gateway dependency at runtime.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public delegate IPaymentGateway ServiceResolver(string key);

        /// <summary>
        /// Method to register dependency.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        public static void RegisterBusinessDependency(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<IExpensivePaymentGateway, ExpensivePaymentGateway>();
            services.AddTransient<ICheapPaymentGateway, CheapPaymentGateway>();
            services.AddTransient<IPremiumPaymentGateway,PremiumPaymentGateway>();

            services.AddTransient<ServiceResolver>(serviceProvider => key =>
            {
                switch (key)
                {
                    case Constants.CHEAP:
                        return serviceProvider.GetService<ICheapPaymentGateway>();
                    case Constants.EXPENSIVE:
                        return serviceProvider.GetService<IExpensivePaymentGateway>();
                    case Constants.PREMIUM:
                        return serviceProvider.GetService<IPremiumPaymentGateway>();
                    default:
                        throw new KeyNotFoundException(); // or maybe return null, up to you
                }
            });

            services.AddTransient<IPaymentsRepository, PaymentsRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<PaymentDbContext>(opt =>
            {
                opt
                    .UseSqlServer(connectionString, x => x.MigrationsAssembly("PaymentDataLayer"));
                // enable below code for lazy loading
                // opt.UseLazyLoadingProxies(); 
            });
        }
    }
}

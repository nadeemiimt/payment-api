using Microsoft.Extensions.DependencyInjection;
using PaymentCommon.Interfaces;
using PaymentCommon.Logs;

namespace PaymentApi.DI
{
    public static class Register
    {
        /// <summary>
        /// Register dependency.
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterDependency(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddTransient<IPaymentBusiness, PaymentBusiness.Payment.PaymentBusiness>();
        }
    }
}

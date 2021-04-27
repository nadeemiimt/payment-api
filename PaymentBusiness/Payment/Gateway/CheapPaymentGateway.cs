using System.Threading.Tasks;
using PaymentCommon.Interfaces;
using PaymentCommon.Models;
using PaymentCommon.Models.Response;

namespace PaymentBusiness.Payment.Gateway
{
    /// <summary>
    /// Cheap Payment gateway.
    /// </summary>
    public class CheapPaymentGateway : ICheapPaymentGateway
    {
        /// <summary>
        /// Is payment gateway available (for now we are returning true).
        /// </summary>
        /// <returns></returns>
        public Task<bool> IsAvailableAsync()
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Process payment.
        /// </summary>
        /// <param name="paymentRequest">Payment request.</param>
        /// <returns></returns>
        public Task<PaymentStatus> ProcessPaymentAsync(PaymentRequestModel paymentRequest)
        {
            return Task.FromResult(PaymentStatus.Processed);
        }
    }
}

using System.Threading.Tasks;
using PaymentCommon.Models;
using PaymentCommon.Models.Response;

namespace PaymentCommon.Interfaces
{
    public interface IPaymentGateway
    {
        Task<PaymentStatus> ProcessPaymentAsync(PaymentRequestModel paymentRequest);
        Task<bool> IsAvailableAsync();
    }
}

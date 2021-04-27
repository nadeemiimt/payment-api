using System.Threading.Tasks;
using PaymentCommon.Models;
using PaymentCommon.Models.Response;

namespace PaymentCommon.Interfaces
{
    public interface IPaymentBusiness
    {
        Task<PaymentStatus> ProcessPaymentAsync(PaymentRequestModel paymentRequestModel);
    }
}

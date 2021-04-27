using PaymentEntities.Entities;
using PaymentEntities.Interfaces;

namespace PaymentDataLayer
{
    public class PaymentsRepository : GenericRepository<Payment>, IPaymentsRepository
    {
        /// <summary>
        /// Payment repository utilizing generic repository.
        /// </summary>
        /// <param name="context"></param>
        public PaymentsRepository(PaymentDbContext context) : base(context)
        {

        }
    }
}

using System;
using PaymentEntities.Interfaces;

namespace PaymentDataLayer
{
    /// <summary>
    /// Unit of work implementation.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PaymentDbContext _context;

        private IPaymentsRepository _paymentsRepository;

        public UnitOfWork(PaymentDbContext paymentDbContext)
        {
            this._context = paymentDbContext;
        }

        /// <summary>
        /// Payment repository reference.
        /// </summary>
        public IPaymentsRepository PaymentsRepository
        {
            get { return _paymentsRepository ??= new PaymentsRepository(_context); }
        }

        /// <summary>
        /// On complete saves all changes in reference.
        /// </summary>
        /// <returns></returns>
        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}

using System;

namespace PaymentEntities.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPaymentsRepository PaymentsRepository { get; }
        int Complete();
    }
}

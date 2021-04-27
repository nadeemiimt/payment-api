using System.Threading.Tasks;

namespace PaymentEntities.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
    }
}

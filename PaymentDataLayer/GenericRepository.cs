using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentEntities.Interfaces;

namespace PaymentDataLayer
{
    /// <summary>
    /// Generic repo containing common CRUD methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Db entities.
        /// </summary>
        protected readonly DbSet<T> _entities;

        protected GenericRepository(PaymentDbContext context)
        {
            _entities = context.Set<T>();

        }

        /// <summary>
        /// Creates resource.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
        }

        /// <summary>
        /// Update resource.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task UpdateAsync(T entity)
        {
            return Task.FromResult(_entities.Update(entity));
        }

        // Delete and Get are left purposefully as they are not required.
    }
}

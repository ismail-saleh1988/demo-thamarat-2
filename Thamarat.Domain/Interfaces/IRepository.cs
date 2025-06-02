using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thamarat.Domain.Interfaces
{
    using System.Linq.Expressions;

    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            string? includeProperties = null
        );
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }

}

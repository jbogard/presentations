using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoUniversity.Services
{
    public interface IRepository<T>
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task SaveChangesAsync();

        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> FindAllAsync();
    }
}
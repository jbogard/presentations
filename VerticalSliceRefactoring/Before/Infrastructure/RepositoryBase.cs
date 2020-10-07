using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Services;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected SchoolContext DbContext { get; }

        public Repository(SchoolContext dbContext) 
            => DbContext = dbContext;

        public async Task<T> AddAsync(T entity)
        {
            await DbContext.Set<T>().AddAsync(entity);
            await DbContext.SaveChangesAsync();

            return entity;
        }

        public Task UpdateAsync(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;

            return DbContext.SaveChangesAsync();
        }

        public Task SaveChangesAsync() => DbContext.SaveChangesAsync();

        public virtual async Task<T> GetByIdAsync(int id) => await DbContext.Set<T>().FindAsync(id);
        public virtual async Task<IReadOnlyList<T>> FindAllAsync() => await DbContext.Set<T>().ToListAsync();
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Infrastructure
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(SchoolContext dbContext) : base(dbContext)
        {
        }

        public async Task<IList<Department>> ListByNameAsync() => 
            await DbContext
                .Departments
                .OrderBy(d => d.Name)
                .AsNoTracking()
                .ToListAsync();
    }
}
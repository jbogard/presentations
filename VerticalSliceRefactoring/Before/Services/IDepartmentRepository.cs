using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoUniversity.Models;

namespace ContosoUniversity.Services
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<IList<Department>> ListByNameAsync();
    }
}
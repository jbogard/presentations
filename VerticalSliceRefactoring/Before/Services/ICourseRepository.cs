using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoUniversity.Models;

namespace ContosoUniversity.Services
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<IList<Course>> ListWithDepartmentsAsync();
        Task<Course> FindWithDepartmentById(int id);
    }
}
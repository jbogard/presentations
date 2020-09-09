using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoUniversity.Data;
using ContosoUniversity.Models;
using ContosoUniversity.Services;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversity.Infrastructure
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(SchoolContext dbContext) : base(dbContext)
        {
        }

        //public override Task<Course> GetByIdAsync(int id) => DbContext.Courses.FindAsync();

        public async Task<IList<Course>> ListWithDepartmentsAsync() =>
            await DbContext
                .Courses
                .Include(c => c.Department)
                .AsNoTracking()
                .ToListAsync();

        public Task<Course> FindWithDepartmentById(int id) =>
            DbContext
                .Courses
                .AsNoTracking()
                .Include(c => c.Department)
                .FirstOrDefaultAsync(m => m.CourseID == id);
    }
}
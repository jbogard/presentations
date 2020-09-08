using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public IndexModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public Result Data { get; set; }

        public class Result
        {
            public List<Course> Courses { get; set; }

            public class Course
            {
                [Display(Name = "Number")]
                public int CourseID { get; set; }
                public string Title { get; set; }
                public int Credits { get; set; }
                [Display(Name = "Department")]
                public string DepartmentName { get; set; }
            }
        }

        public async Task OnGetAsync()
        {
            Data = new Result
            {
                Courses = await _context.Courses
                    .Select(c => new Result.Course
                    {
                        Credits = c.Credits,
                        DepartmentName = c.Department.Name,
                        CourseID = c.CourseID,
                        Title = c.Title
                    })
                    .ToListAsync()
            };
        }
    }
}

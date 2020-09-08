using System.ComponentModel.DataAnnotations;
using System.Linq;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ContosoUniversity.Pages.Courses
{
    public class DetailsModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public DetailsModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        public Model Data { get; set; }

        public class Model
        {
            [Display(Name = "Number")]
            public int CourseID { get; set; }
            public string Title { get; set; }
            public int Credits { get; set; }
            [Display(Name = "Department")]
            public string DepartmentName { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Data = await _context.Courses
                .Select(c => new Model
                {
                    CourseID = c.CourseID,
                    Title = c.Title,
                    Credits = c.Credits,
                    DepartmentName = c.Department.Name
                })
                .FirstOrDefaultAsync(m => m.CourseID == id);

            if (Data == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

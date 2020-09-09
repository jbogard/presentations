using System.ComponentModel.DataAnnotations;
using System.Linq;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ContosoUniversity.Pages.Courses
{
    public class EditModel : PageModel
    {
        private readonly ContosoUniversity.Data.SchoolContext _context;

        public EditModel(ContosoUniversity.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Model Data { get; set; }

        public class Model
        {
            [Display(Name = "Number")]
            public int CourseID { get; set; }
            [StringLength(50, MinimumLength = 3)]
            [Required]
            public string Title { get; set; }
            [Range(0, 5)]
            [Required]
            public int? Credits { get; set; }
            public int DepartmentID { get; set; }
            public SelectList DepartmentNameSL { get; set; }
            [Display(Name = "Department")]
            public string DepartmentName { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Data = await _context.Courses.Select(c => new Model
                {
                    CourseID = c.CourseID,
                    Credits = c.Credits,
                    DepartmentID = c.DepartmentID,
                    Title = c.Title
                })
                .FirstOrDefaultAsync(m => m.CourseID == id);

            if (Data == null)
            {
                return NotFound();
            }

            // Select current DepartmentID.
            PopulateDepartmentsDropDownList(Data.DepartmentID);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseToUpdate = await _context.Courses.FindAsync(id);

            if (courseToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Course>(
                 courseToUpdate,
                 "data",   // Prefix for form value.
                   c => c.Credits, c => c.DepartmentID, c => c.Title))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            // Select DepartmentID if TryUpdateModelAsync fails.
            PopulateDepartmentsDropDownList(courseToUpdate.DepartmentID);
            return Page();
        }

        public void PopulateDepartmentsDropDownList(object selectedDepartment = null)
        {
            var departmentsQuery = from d in _context.Departments
                orderby d.Name // Sort by name.
                select d;

            Data.DepartmentNameSL = new SelectList(departmentsQuery.AsNoTracking(),
                "DepartmentID", "Name", selectedDepartment);
        }

    }
}

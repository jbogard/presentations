using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ContosoUniversity.Services;

namespace ContosoUniversity.Pages.Courses
{
    public class DetailsModel : PageModel
    {
        private readonly ICourseRepository _repository;

        public DetailsModel(ICourseRepository repository) => _repository = repository;

        public Course Course { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Course = await _repository.FindWithDepartmentById(id.Value);

            if (Course == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}

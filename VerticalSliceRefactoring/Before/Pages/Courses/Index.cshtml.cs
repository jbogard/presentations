using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContosoUniversity.Services;

namespace ContosoUniversity.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ICourseRepository _repository;

        public IndexModel(ICourseRepository repository) => _repository = repository;

        public IList<Course> Courses { get; set; }

        public async Task OnGetAsync()
        {
            Courses = await _repository.ListWithDepartmentsAsync();
        }
    }
}

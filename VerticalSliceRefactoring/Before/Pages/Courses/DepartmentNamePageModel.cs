using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using ContosoUniversity.Services;

namespace ContosoUniversity.Pages.Courses
{
    public class DepartmentNamePageModel : PageModel
    {
        public SelectList DepartmentNameSL { get; set; }

        public async Task PopulateDepartmentsDropDownList(IDepartmentRepository repository,
            object selectedDepartment = null)
        {
            var departments = await repository.ListByNameAsync();

            DepartmentNameSL = new SelectList(departments,
                        "DepartmentID", "Name", selectedDepartment);
        }
    }
}
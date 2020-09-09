using ContosoUniversity.Data;
using ContosoUniversity.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly SchoolContext _context;

        public IndexModel(SchoolContext context)
        {
            _context = context;
        }

        public Result Data { get; private set; }

        public class Result
        {
            public string CurrentSort { get; set; }
            public string NameSortParm { get; set; }
            public string DateSortParm { get; set; }
            public string CurrentFilter { get; set; }
            public string SearchString { get; set; }

            public PaginatedList<Model> Results { get; set; }

            public class Model
            {
                public int ID { get; set; }
                [Display(Name = "First Name")]
                public string FirstMidName { get; set; }
                public string LastName { get; set; }
                public DateTime EnrollmentDate { get; set; }
            }
        }

        public async Task OnGetAsync(string sortOrder,
            string currentFilter, string searchString, int? pageIndex)
        {
            var model = new Result
            {
                CurrentSort = sortOrder,
                NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "",
                DateSortParm = sortOrder == "Date" ? "date_desc" : "Date"
            };

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            model.CurrentFilter = searchString;
            model.SearchString = searchString;

            IQueryable<Student> students = _context.Students;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                                               || s.FirstMidName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default: // Name ascending 
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = pageIndex ?? 1;
            model.Results = await students
                .Select(s => new Result.Model
                {
                    ID = s.ID,
                    FirstMidName = s.FirstMidName,
                    LastName = s.LastName,
                    EnrollmentDate = s.EnrollmentDate
                })
                .PaginatedListAsync(pageNumber, pageSize);

            Data = model;
        }
    }
}

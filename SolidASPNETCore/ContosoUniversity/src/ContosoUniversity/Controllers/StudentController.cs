namespace ContosoUniversity.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using ViewModels;
    using X.PagedList;

    public class StudentController : Controller
    {
        private readonly SchoolContext db;

        public StudentController(SchoolContext db)
        {
            this.db = db;
        }

        // GET: /Student/
        public ViewResult Index(StudentIndexQuery query)
        {
            var model = new StudentIndexResult
            {
                CurrentSort = query.SortOrder,
                NameSortParm = String.IsNullOrEmpty(query.SortOrder) ? "name_desc" : "",
                DateSortParm = query.SortOrder == "Date" ? "date_desc" : "Date",
            };

            if (query.SearchString != null)
            {
                query.Page = 1;
            }
            else
            {
                query.SearchString = query.CurrentFilter;
            }

            model.CurrentFilter = query.SearchString;
            model.SearchString = query.SearchString;

            var students = from s in db.Students
                           select s;
            if (!string.IsNullOrEmpty(query.SearchString))
            {
                students = students.Where(s => s.LastName.Contains(query.SearchString)
                                               || s.FirstMidName.Contains(query.SearchString));
            }
            switch (query.SortOrder)
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
            int pageNumber = query.Page ?? 1;
            model.Results = students
                .Select(s => new StudentIndexModel
                {
                    ID = s.ID,
                    EnrollmentDate = s.EnrollmentDate,
                    FirstMidName = s.FirstMidName,
                    LastName = s.LastName
                })
                .ToPagedList(pageNumber, pageSize);

            return View(model);
        }

        // GET: /Student/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = db.Students
                .Where(s => s.ID == id)
                .Select(s => new StudentDetailsModel
                {
                    ID = s.ID,
                    EnrollmentDate = s.EnrollmentDate,
                    FirstMidName = s.FirstMidName,
                    LastName = s.LastName,
                    Enrollments = s.Enrollments.Select(e =>
                        new StudentDetailsModel.Enrollment
                        {
                            CourseTitle = e.Course.Title,
                            Grade = e.Grade
                        }).ToList()
                })
                .SingleOrDefault();
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // GET: /Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StudentCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    FirstMidName = model.FirstMidName,
                    LastName = model.LastName,
                    EnrollmentDate = model.EnrollmentDate.GetValueOrDefault()
                };
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: /Student/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = db.Students
                .Where(s => s.ID == id)
                .Select(s => new StudentEditModel
                {
                    ID = s.ID,
                    FirstMidName = s.FirstMidName,
                    LastName = s.LastName,
                    EnrollmentDate = s.EnrollmentDate
                })
                .SingleOrDefault();
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: /Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StudentEditModel model)
        {
            if (ModelState.IsValid)
            {
                var student = db.Students.SingleOrDefault(s => s.ID == model.ID);

                student.FirstMidName = model.FirstMidName;
                student.LastName = model.LastName;
                student.EnrollmentDate = model.EnrollmentDate.GetValueOrDefault();

                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
        // GET: /Student/Delete/5
        public IActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            var model = db.Students
                .Where(s => s.ID == id)
                .Select(s => new StudentDeleteModel
                {
                    ID = s.ID,
                    FirstMidName = s.FirstMidName,
                    LastName = s.LastName,
                    EnrollmentDate = s.EnrollmentDate
                })
                .SingleOrDefault();
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: /Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Student student = db.Students.Single(s => s.ID == id);
            db.Students.Remove(student);
            db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
namespace ContosoUniversity.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using DAL;
    using Helpers;
    using Models;
    using PagedList;
    using ViewModels;

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
            if (!String.IsNullOrEmpty(query.SearchString))
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
            int pageNumber = (query.Page ?? 1);
            model.Results = students.ProjectToPagedList<StudentIndexModel>(pageNumber, pageSize);

            return View(model);
        }
        // GET: /Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: /Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName, FirstMidName, EnrollmentDate")]Student student)
        {
            db.Students.Add(student);
            db.SaveChanges();
            return this.RedirectToActionJson("Index");
        }

        // GET: /Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID, LastName, FirstMidName, EnrollmentDate")]Student student)
        {
            db.Entry(student).State = EntityState.Modified;
            db.SaveChanges();
            return this.RedirectToActionJson("Index");
        }
        // GET: /Student/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();

            return this.RedirectToActionJson("Index");
        }
    }
}

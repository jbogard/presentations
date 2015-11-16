namespace ContosoUniversity.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using CollectionJson;
    using Microsoft.AspNet.Mvc;

    [Route("api/instructor")]
    public class InstructorController : Controller
    {
        private SchoolContext db = new SchoolContext();

        [Route("")]
        [HttpGet]
        public async Task<ReadDocument> Index()
        {
            var instructors = await db.Instructors.Include(i => i.OfficeAssignment).ToListAsync();
            var doc = new ReadDocument
            {
                Collection =
                {
                    Href = new Uri(Url.Action("Index")),
                    Version = "1.0"
                }
            };
            foreach (var item in instructors.Select(i => new Item
            {
                Href = Url.Link<InstructorController>(c => c.Details(i.ID)),
                Data = new List<Data>
                {
                    new Data
                    {
                        Name = "last-name",
                        Prompt = "Last Name",
                        Value = i.LastName
                    },
                    new Data
                    {
                        Name = "first-name",
                        Prompt = "First Name",
                        Value = i.FirstMidName
                    },
                    new Data
                    {
                        Name = "hire-date",
                        Prompt = "Hire Date",
                        Value = i.HireDate.ToShortDateString()
                    },
                    new Data
                    {
                        Name = "office",
                        Prompt = "Office",
                        Value = i.OfficeAssignment == null ? null : i.OfficeAssignment.Location
                    },
                },
                Links = new List<Link>
                {
                    new Link { Href = new Uri(Url.Link<InstructorController>(c => c.Courses(i.ID)) + "/courses"), Prompt = "Courses", Rel = "courses" }
                }
            }))
            {
                doc.Collection.Items.Add(item);
            }

            return doc.ToHttpResponseMessage();
        }

        [Route("{id}")]
        [HttpGet]
        public HttpResponseMessage Details(int id)
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [Route("{id}/courses")]
        [HttpGet]
        public async Task<HttpResponseMessage> Courses(int id)
        {
            var instructor = await db.Instructors.Where(i => i.ID == id).Include(i => i.Courses).SingleAsync();
            var courses = instructor.Courses;

            var doc = new ReadDocument
            {
                Collection =
                {
                    Href = new Uri(Url.Link<InstructorController>(c => c.Courses(id)) + "/courses"),
                    Version = "1.0"
                }
            };

            foreach (var item in courses.Select(c => new Item
            {
                Data = new List<Data>
                {
                    new Data { Name = "number", Prompt = "Number", Value = c.CourseID.ToString() },
                    new Data { Name = "title", Prompt = "Title", Value = c.Title },
                    new Data { Name = "dept", Prompt = "Department", Value = c.Department.Name }
                },
                Links = new List<Link>
                {
                    new Link
                    {
                        Href = new Uri(string.Format(Url.Link<InstructorController>(x => x.Index()) + "{0}/courses/{1}/students", id, c.CourseID)),
                        Prompt = "Students",
                        Rel = "students"
                    }
                }
            }))
            {
                doc.Collection.Items.Add(item);
            }

            return doc.ToHttpResponseMessage();
        }

        [Route("{id}/courses/{courseId}/students")]
        [HttpGet]
        public async Task<HttpResponseMessage> Students(int id, int courseId)
        {
            var course = await db.Courses.Where(c => c.CourseID == courseId).Include(c => c.Enrollments).SingleAsync();
            var enrollments = course.Enrollments;

            var doc = new ReadDocument
            {
                Collection =
                {
                    Href = new Uri(string.Format(Url.Link<InstructorController>(c => c.Index()) + "/courses/{0}/students", courseId)),
                    Version = "1.0"
                }
            };

            foreach (var item in enrollments.Select(e => new Item
            {
                Data = new List<Data>
                {
                    new Data { Name = "full-name", Prompt = "Name", Value = e.Student.FullName },
                    new Data { Name = "grade", Prompt = "Grade", Value = e.Grade.ToString() },
                }
            }))
            {
                doc.Collection.Items.Add(item);
            }

            return doc.ToHttpResponseMessage();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
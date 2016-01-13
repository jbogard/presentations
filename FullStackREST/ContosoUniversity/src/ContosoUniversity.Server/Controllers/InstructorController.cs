using ContosoUniversity.Server.Models;
using Microsoft.ApplicationInsights.AspNet.Extensions;

namespace ContosoUniversity.Server.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using CollectionJson;
    using Microsoft.AspNet.Mvc;
    using Microsoft.Data.Entity;
    using System.Linq;

    [Route("api/instructor")]
    public class InstructorController : Controller
    {
        private readonly SchoolContext db;

        public InstructorController(SchoolContext db)
        {
            this.db = db;
        }

        [Route("")]
        [HttpGet]
        public async Task<ReadDocument> Index()
        {
            var instructors = await db.Instructors.Include(i => i.OfficeAssignment).ToListAsync();
            var doc = new ReadDocument
            {
                Collection =
                {
                    Href = new Uri(Url.Action("Index", "Instructor", null, Request.GetUri().Scheme, null)),
                    Version = "1.0"
                }
            };
            foreach (var item in instructors.Select(i => new Item
            {
                Href = new Uri(Url.Action("Details", "Instructor", new { id=i.ID }, Request.GetUri().Scheme, null)),
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
                        Value = i.OfficeAssignment?.Location
                    },
                },
                Links = new List<Link>
                {
                    new Link { Href = new Uri(Url.Action("Courses", "Instructor", new { id=i.ID }, Request.GetUri().Scheme, null)), Prompt = "Courses", Rel = "courses" }
                }
            }))
            {
                doc.Collection.Items.Add(item);
            }

            return doc;
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult Details(int id)
        {
            return HttpNotFound();
        }

        [Route("{id}/courses")]
        [HttpGet]
        public async Task<ReadDocument> Courses(int id)
        {
            var courses = await db.CourseInstructors.Where(i => i.InstructorID == id).Include(ci => ci.Course).ThenInclude(c => c.Department).ToListAsync();

            var doc = new ReadDocument
            {
                Collection =
                {
                    Href = new Uri(Url.Action("Courses", "Instructor", new {id}, Request.GetUri().Scheme, null)),
                    Version = "1.0"
                }
            };

            foreach (var item in courses.Select(c => new Item
            {
                Data = new List<Data>
                {
                    new Data { Name = "number", Prompt = "Number", Value = c.CourseID.ToString() },
                    new Data { Name = "title", Prompt = "Title", Value = c.Course.Title },
                    new Data { Name = "dept", Prompt = "Department", Value = c.Course.Department?.Name }
                },
                Links = new List<Link>
                {
                    new Link
                    {
                        Href = new Uri(Url.Action("Students", "Instructor", new { id, courseId = c.CourseID}, Request.GetUri().Scheme, null)),
                        Prompt = "Students",
                        Rel = "students"
                    }
                }
            }))
            {
                doc.Collection.Items.Add(item);
            }

            return doc;
        }

        [Route("{id}/courses/{courseId}/students")]
        [HttpGet]
        public async Task<ReadDocument> Students(int id, int courseId)
        {
            var course = await db.Courses.Where(c => c.CourseID == courseId).Include(c => c.Enrollments).ThenInclude(e => e.Student).SingleAsync();
            var enrollments = course.Enrollments;

            var doc = new ReadDocument
            {
                Collection =
                {
                    Href = new Uri(Url.Action("Students", "Instructor", new {id, courseId}, Request.GetUri().Scheme, null)),
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

            return doc;
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
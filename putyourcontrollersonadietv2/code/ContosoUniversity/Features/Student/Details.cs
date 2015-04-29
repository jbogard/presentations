namespace ContosoUniversity.Features.Student
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using DAL;
    using Infrastructure;
    using MediatR;
    using Models;

    public class Details
    {
        public class Query : IAsyncRequest<Model>
        {
            [Required]
            public int? Id { get; set; }
        }

        public class Model
        {
            public int ID { get; set; }
            [Display(Name = "First Name")]
            public string FirstMidName { get; set; }
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
            [Display(Name = "Enrollment Date")]
            public DateTime EnrollmentDate { get; set; }
            public List<Enrollment> Enrollments { get; set; }

            public class Enrollment
            {
                public string CourseTitle { get; set; }
                public Grade? Grade { get; set; }
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Model>
        {
            private readonly SchoolContext db;

            public QueryHandler(SchoolContext db)
            {
                this.db = db;
            }

            public async Task<Model> Handle(Query message)
            {
                var student = await db.Students
                    .Where(s => s.ID == message.Id)
                    .ProjectToSingleOrDefaultAsync<Model>();

                return student;
            }
        }


    }
}
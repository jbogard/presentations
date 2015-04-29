namespace ContosoUniversity.Features.Student
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using DAL;
    using Infrastructure;
    using MediatR;

    public class Delete
    {
        public class Command : IAsyncRequest
        {
            public int ID { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "First Name")]
            public string FirstMidName { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
            [Display(Name = "Enrollment Date")]
            public DateTime? EnrollmentDate { get; set; }
        }

        public class Query : IAsyncRequest<Command>
        {
            [Required]
            public int? Id { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Command>
        {
            private readonly SchoolContext db;

            public QueryHandler(SchoolContext db)
            {
                this.db = db;
            }

            public async Task<Command> Handle(Query message)
            {
                var student = await db.Students
                    .Where(s => s.ID == message.Id)
                    .ProjectToSingleOrDefaultAsync<Command>();

                return student;
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly SchoolContext db;

            public CommandHandler(SchoolContext db)
            {
                this.db = db;
            }

            protected override async Task HandleCore(Command message)
            {
                var student = await db.Students.FindAsync(message.ID);

                db.Students.Remove(student);
            }
        }

    }
}
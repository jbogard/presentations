namespace ContosoUniversity.Features.Student
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using DAL;
    using Infrastructure;
    using MediatR;
    using ViewModels;

    public class Edit
    {
        public class Query : IAsyncRequest<Command>
        {
            [Required]
            public int? Id { get; set; }
        }

        public class Command : StudentModel, IAsyncRequest
        {
            public int ID { get; set; }
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

                Mapper.Map(message, student);
            }
        }

    }
}
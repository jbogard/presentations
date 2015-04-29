namespace ContosoUniversity.Features.Student
{
    using AutoMapper;
    using DAL;
    using MediatR;
    using ViewModels;
    using Models;

    public class Create
    {
        public class Command : StudentModel, IRequest
        {
        }

        public class Handler : RequestHandler<Command>
        {
            private readonly SchoolContext db;

            public Handler(SchoolContext db)
            {
                this.db = db;
            }

            protected override void HandleCore(Command message)
            {
                var student = Mapper.Map<Command, Student>(message);

                db.Students.Add(student);
            }
        }


    }
}
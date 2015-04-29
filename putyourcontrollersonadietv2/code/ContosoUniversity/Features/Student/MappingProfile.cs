namespace ContosoUniversity.Features.Student
{
    using AutoMapper;
    using Models;

    public class StudentMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Student, Index.Model>();
            CreateMap<Student, Details.Model>();
            CreateMap<Enrollment, Details.Model.Enrollment>();
            CreateMap<Create.Command, Student>();

            CreateMap<Student, Edit.Command>().ReverseMap();
            CreateMap<Student, Delete.Command>();
        }
    }
}
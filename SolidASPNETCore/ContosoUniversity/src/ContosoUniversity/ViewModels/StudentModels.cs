namespace ContosoUniversity.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Models;
    using X.PagedList;
    using AutoMapper;

    public class StudentIndexQuery
    {
        public string SortOrder { get; set; }
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }
        public int? Page { get; set; }
    }

    public class StudentIndexResult
    {
        public string CurrentSort { get; set; }
        public string NameSortParm { get; set; }
        public string DateSortParm { get; set; }
        public string CurrentFilter { get; set; }
        public string SearchString { get; set; }

        public IPagedList<StudentIndexModel> Results { get; set; }
    }

    public class StudentIndexModel
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
    }

    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            CreateMap<Student, StudentIndexModel>();
        }
    }

}
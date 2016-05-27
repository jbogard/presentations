namespace ContosoUniversity.Models
{
    public class CourseInstructor
    {
        public int CourseID { get; set; } 
        public Course Course { get; set; }

        public int InstructorID { get; set; }
        public Instructor Instructor { get; set; }
    }
}
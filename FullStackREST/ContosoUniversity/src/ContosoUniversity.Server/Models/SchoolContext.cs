using ContosoUniversity.Server.Models;
using Microsoft.Data.Entity;

namespace ContosoUniversity.Server.Models
{
    public class SchoolContext : DbContext
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<CourseInstructor> CourseInstructors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseInstructor>()
                .HasKey(t => new { t.CourseID, t.InstructorID });

            //modelBuilder.Entity<PostTag>()
            //    .HasOne(pt => pt.Post)
            //    .WithMany(p => p.PostTags)
            //    .HasForeignKey(pt => pt.PostId);

            //modelBuilder.Entity<PostTag>()
            //    .HasOne(pt => pt.Tag)
            //    .WithMany(t => t.PostTags)
            //    .HasForeignKey(pt => pt.TagId);
        }
    }
}
### Step 1 - Simple Reads

Convert Courses\Index.cshtml to use viewmodels

Examine Index.cshtml

Note the data being used

Create model to represent that data

```csharp
public class Result
{
    public List<Course> Courses { get; set; }

    public class Course
    {
        [Display(Name = "Number")]
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        [Display(Name = "Department")]
        public string DepartmentName { get; set; }
    }
}
```

Rename property to Data

Alter OnGetAsync:

```csharp
Data = new Result
{
    Courses = await _context.Courses
        .Select(c => new Result.Course
        {
            Credits = c.Credits,
            DepartmentName = c.Department.Name,
            CourseID = c.CourseID,
            Title = c.Title
        })
        .ToListAsync()
};

using System.Linq;
```

Fix Index.cshtml

### Step 2 - Another Simple Read - Details

Rename to Data

```csharp
public class Model
{
    [Display(Name = "Number")]
    public int CourseID { get; set; }
    public string Title { get; set; }
    public int Credits { get; set; }
    [Display(Name = "Department")]
    public string DepartmentName { get; set; }
}
```

Fix `OnGetAsync` and view

### Step 3 - One-to-many

Students\Detail.cshtml

Rename to Data

```csharp
public class Model
{
    public int ID { get; set; }
    [Display(Name = "First Name")]
    public string FirstMidName { get; set; }
    public string LastName { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public List<Enrollment> Enrollments { get; set; }

    public class Enrollment
    {
        public string CourseTitle { get; set; }
        public Grade? Grade { get; set; }
    }
}
```

### Step 4 - Complex Read

Students\Index.cshtml

```csharp
public class Result
{
    public string CurrentSort { get; set; }
    public string NameSort { get; set; }
    public string DateSort { get; set; }
    public string CurrentFilter { get; set; }
    public string SearchString { get; set; }

    public PaginatedList<Model> Students { get; set; }

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
    }
}
```

Create property Data

Fix view

### Step 5 - AutoMapper

Profiles and replacing with ProjectTo

### Step 6 - Editing

Model:

```csharp
public class Model
{
    public int ID { get; set; }
    [Required]
    [StringLength(50)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
    [Column("FirstName")]
    [Display(Name = "First Name")]
    public string FirstMidName { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [Display(Name = "Enrollment Date")]
    public DateTime EnrollmentDate { get; set; }
}
```

OnGetAsync:

```csharp
public async Task<IActionResult> OnGetAsync(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    Data = await _context.Students.Select(s => new Model
    {
        ID = s.ID,
        FirstMidName = s.FirstMidName,
        LastName = s.LastName,
        EnrollmentDate = s.EnrollmentDate
    }).SingleOrDefaultAsync(s => s.ID == id);

    if (Data == null)
    {
        return NotFound();
    }
    return Page();
}
```

OnPostAsync

```csharp
public async Task<IActionResult> OnPostAsync(int id)
{
    var studentToUpdate = await _context.Students.FindAsync(id);

    if (studentToUpdate == null)
    {
        return NotFound();
    }

    studentToUpdate.FirstMidName = Data.FirstMidName;
    studentToUpdate.LastName = Data.LastName;
    studentToUpdate.EnrollmentDate = Data.EnrollmentDate;
    await _context.SaveChangesAsync();
    return RedirectToPage("./Index");
}
```

### Step 7 - MediatR
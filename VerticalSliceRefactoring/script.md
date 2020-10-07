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

AutoMapAttribute and services.AddAutoMapper


### Step 6 - Editing

Courses\Edit.cshtml

Model:

```csharp
public class Model
{
    [Display(Name = "Number")]
    public int CourseID { get; set; }
    [StringLength(50, MinimumLength = 3)]
    [Required]
    public string Title { get; set; }
    [Range(0, 5)]
    [Required]
    public int? Credits { get; set; }
    public int DepartmentID { get; set; }
    public SelectList DepartmentNameSL { get; set; }
    [Display(Name = "Department")]
    public string DepartmentName { get; set; }
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

    Data = await _context.Courses.Select(c => new Model
        {
            CourseID = c.CourseID,
            Credits = c.Credits,
            DepartmentID = c.DepartmentID,
            Title = c.Title
        })
        .FirstOrDefaultAsync(m => m.CourseID == id);

    if (Data == null)
    {
        return NotFound();
    }

    // Select current DepartmentID.
    PopulateDepartmentsDropDownList(Data.DepartmentID);
    return Page();
}

private void PopulateDepartmentsDropDownList(object selectedDepartment = null)
{
    var departmentsQuery = from d in _context.Departments
        orderby d.Name // Sort by name.
        select d;

    Data.DepartmentNameSL = new SelectList(departmentsQuery.AsNoTracking(),
        "DepartmentID", "Name", selectedDepartment);
}
```

OnPostAsync

```csharp
public async Task<IActionResult> OnPostAsync(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    var courseToUpdate = await _context.Courses.FindAsync(id);

    if (courseToUpdate == null)
    {
        return NotFound();
    }

    if (await TryUpdateModelAsync<Course>(
         courseToUpdate,
         "data",   // Prefix for form value.
           c => c.Credits, c => c.DepartmentID, c => c.Title))
    {
        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }

    // Select DepartmentID if TryUpdateModelAsync fails.
    PopulateDepartmentsDropDownList(courseToUpdate.DepartmentID);
    return Page();
}
```

### Step 7 - MediatR
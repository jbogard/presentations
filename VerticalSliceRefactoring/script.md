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


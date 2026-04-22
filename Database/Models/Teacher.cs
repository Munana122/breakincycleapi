using System;
using System.Collections.Generic;

namespace breakincycleapi.Database.Models;

public partial class Teacher
{
    public Guid TeacherId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phonenumber { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Coursename { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public DateTime Lastactive { get; set; }

    public virtual ICollection<TeacherCourse> TeacherCourses { get; set; } = new List<TeacherCourse>();
}

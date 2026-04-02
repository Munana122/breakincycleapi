using System;
using System.Collections.Generic;

namespace breakincycleapi.Database.Models;

public partial class Course
{
    public Guid CourseId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public DateTime Lastactive { get; set; }

    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();

    public virtual ICollection<TeacherCourse> TeacherCourses { get; set; } = new List<TeacherCourse>();
}

using System;
using System.Collections.Generic;

namespace breakincycleapi.Database.Models;

public partial class TeacherCourse
{
    public long TeacherCourseId { get; set; }

    public Guid TeacherId { get; set; }

    public Guid CourseId { get; set; }

    public DateTime AssignedAt { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace breakincycleapi.Database.Models;

public partial class Student
{
    public Guid StudentId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phonenumbar { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime Createdat { get; set; }

    public DateTime Lastactive { get; set; }

    public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();
}

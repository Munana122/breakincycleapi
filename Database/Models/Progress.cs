using System;
using System.Collections.Generic;

namespace breakincycleapi.Database.Models;

public partial class Progress
{
    public long ProgressId { get; set; }

    public Guid StudentId { get; set; }

    public Guid CourseId { get; set; }

    public string ProgressStatus { get; set; } = string.Empty;
    public DateTime LastUpdated { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}

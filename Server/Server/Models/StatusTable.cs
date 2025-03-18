using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class StatusTable
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateOnly? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual ICollection<PerfomanceReview> PerfomanceReviews { get; set; } = new List<PerfomanceReview>();
}

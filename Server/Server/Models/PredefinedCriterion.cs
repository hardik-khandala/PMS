using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class PredefinedCriterion
{
    public int CriteriaId { get; set; }

    public string CriteriaName { get; set; } = null!;

    public string Details { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateOnly CreatedAt { get; set; }

    public int? ModifyBy { get; set; }

    public DateOnly? ModifyAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual Employee? ModifyByNavigation { get; set; }

    public virtual ICollection<PerfomanceReview> PerfomanceReviews { get; set; } = new List<PerfomanceReview>();
}

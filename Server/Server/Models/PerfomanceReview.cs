using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class PerfomanceReview
{
    public int ReviewId { get; set; }

    public int EmpId { get; set; }

    public int ManagerId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public int CriteriaId { get; set; }

    public int SelfRating { get; set; }

    public string Strength { get; set; } = null!;

    public string Improvement { get; set; } = null!;

    public int StatusId { get; set; }

    public int? ManagerRating { get; set; }

    public string? ManagerFeedback { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public int? ModifyBy { get; set; }

    public DateTime? ModifyAt { get; set; }

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual PredefinedCriterion Criteria { get; set; } = null!;

    public virtual Employee Emp { get; set; } = null!;

    public virtual Employee Manager { get; set; } = null!;

    public virtual Employee? ModifyByNavigation { get; set; }

    public virtual StatusTable Status { get; set; } = null!;
}

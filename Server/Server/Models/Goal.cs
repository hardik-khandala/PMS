using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class Goal
{
    public int GoalId { get; set; }

    public int EmpId { get; set; }

    public string Title { get; set; } = null!;

    public string Detail { get; set; } = null!;

    public int StatusId { get; set; }

    public int CreatedBy { get; set; }

    public DateOnly CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public int? ModifyBy { get; set; }

    public DateOnly? ModifyAt { get; set; }

    public DateOnly? DueDate { get; set; }

    public virtual Employee CreatedByNavigation { get; set; } = null!;

    public virtual Employee Emp { get; set; } = null!;

    public virtual Employee? ModifyByNavigation { get; set; }

    public virtual StatusTable Status { get; set; } = null!;
}

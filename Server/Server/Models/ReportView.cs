using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class ReportView
{
    public int ReviewId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string ManagerName { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string CriteriaName { get; set; } = null!;

    public int SelfRating { get; set; }

    public string Strength { get; set; } = null!;

    public string Improvement { get; set; } = null!;

    public int? ManagerRating { get; set; }

    public string? ManagerFeedback { get; set; }

    public int DeptId { get; set; }
}

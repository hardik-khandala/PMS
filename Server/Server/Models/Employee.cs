using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class Employee
{
    public int EmpId { get; set; }

    public string EmpName { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int DeptId { get; set; }

    public int RoleId { get; set; }

    public DateTime? JoiningDate { get; set; }

    public bool? IsDeleted { get; set; }

    public int? ManagerId { get; set; }

    public int? CreatedBy { get; set; }

    public DateOnly? ModifyAt { get; set; }

    public int? ModiftyBy { get; set; }

    public virtual ICollection<AuditLog> AuditLogChangedByNavigations { get; set; } = new List<AuditLog>();

    public virtual ICollection<AuditLog> AuditLogEmps { get; set; } = new List<AuditLog>();

    public virtual Employee? CreatedByNavigation { get; set; }

    public virtual Department Dept { get; set; } = null!;

    public virtual ICollection<Goal> GoalCreatedByNavigations { get; set; } = new List<Goal>();

    public virtual ICollection<Goal> GoalEmps { get; set; } = new List<Goal>();

    public virtual ICollection<Goal> GoalModifyByNavigations { get; set; } = new List<Goal>();

    public virtual ICollection<Employee> InverseCreatedByNavigation { get; set; } = new List<Employee>();

    public virtual ICollection<Employee> InverseManager { get; set; } = new List<Employee>();

    public virtual ICollection<Employee> InverseModiftyByNavigation { get; set; } = new List<Employee>();

    public virtual Employee? Manager { get; set; }

    public virtual Employee? ModiftyByNavigation { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PerfomanceReview> PerfomanceReviewCreatedByNavigations { get; set; } = new List<PerfomanceReview>();

    public virtual ICollection<PerfomanceReview> PerfomanceReviewEmps { get; set; } = new List<PerfomanceReview>();

    public virtual ICollection<PerfomanceReview> PerfomanceReviewManagers { get; set; } = new List<PerfomanceReview>();

    public virtual ICollection<PerfomanceReview> PerfomanceReviewModifyByNavigations { get; set; } = new List<PerfomanceReview>();

    public virtual ICollection<PredefinedCriterion> PredefinedCriterionCreatedByNavigations { get; set; } = new List<PredefinedCriterion>();

    public virtual ICollection<PredefinedCriterion> PredefinedCriterionModifyByNavigations { get; set; } = new List<PredefinedCriterion>();

    public virtual RoleTable Role { get; set; } = null!;

    public virtual ICollection<StatusTable> StatusTables { get; set; } = new List<StatusTable>();
}

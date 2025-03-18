using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class AuditLog
{
    public int LogId { get; set; }

    public int? EmpId { get; set; }

    public string Action { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public int? ChangedBy { get; set; }

    public DateTime? ChangedAt { get; set; }

    public virtual Employee? ChangedByNavigation { get; set; }

    public virtual Employee? Emp { get; set; }
}

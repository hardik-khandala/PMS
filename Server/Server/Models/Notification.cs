using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public int EmpId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public bool? IsRead { get; set; }

    public virtual Employee Emp { get; set; } = null!;
}

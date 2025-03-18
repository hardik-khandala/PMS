using System;
using System.Collections.Generic;

namespace Server.Models;

public partial class RoleTable
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

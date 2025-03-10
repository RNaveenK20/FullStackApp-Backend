using System;
using System.Collections.Generic;

namespace EmployeeDemo.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Salary { get; set; }

    public string Location { get; set; } = null!;

    public int? Deptno { get; set; }

    public virtual Department? DeptnoNavigation { get; set; }
}

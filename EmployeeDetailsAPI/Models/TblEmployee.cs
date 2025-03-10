using System;
using System.Collections.Generic;

namespace EmployeeDetailsAPI.Models;

public partial class TblEmployee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Salary { get; set; }

    public string Location { get; set; } = null!;

    public int? Deptno { get; set; }

    public virtual TblDepartment? DeptnoNavigation { get; set; }
}

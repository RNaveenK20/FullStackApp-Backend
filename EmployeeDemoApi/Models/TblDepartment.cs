using System;
using System.Collections.Generic;

namespace EmployeeDemoApi.Models;

public partial class TblDepartment
{
    public int Deptid { get; set; }

    public string? Dname { get; set; }

    public virtual ICollection<TblEmployee> TblEmployees { get; set; } = new List<TblEmployee>();
}

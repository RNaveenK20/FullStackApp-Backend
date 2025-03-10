using EmployerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly DemodbContext _context;

        public EmployeeController(DemodbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<TblEmployee> tblEmployees = _context.TblEmployees.ToList();
            List<Employee> employees = tblEmployees.Select(e => new Employee
            {
                Id = e.Id,
                Name = e.Name,
                sal = e.Salary,
                Location = e.Location,
                deptNum = e.Deptno ?? 0
            }).ToList();

            return Ok(employees);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Employee employee)
        {
            TblEmployee tblEmployee = new TblEmployee
            {
                Id = employee.Id,
                Name = employee.Name,
                Salary = employee.sal,
                Location = employee.Location,
                Deptno = employee.deptNum
            };
            _context.TblEmployees.Add(tblEmployee);
            _context.SaveChanges();
            return Ok();
        }
    }
}

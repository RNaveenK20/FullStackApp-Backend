using EmployeeDemoApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeDemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        [HttpGet]
        public List<Employee> Get()
        {
            using (DemodbContext db = new DemodbContext())
            {
                return db.TblEmployees.Select(e => new Employee
                {
                    Id = e.Id,
                    Name = e.Name,
                    Sal = e.Salary,
                    Location = e.Location,
                    deptNum = (int)e.Deptno
                }).ToList();
            }
        }
        [HttpPost]
        public void Post([FromBody] Employee emp)
        {
            using (DemodbContext db = new DemodbContext())
            {
                TblEmployee tblEmployee = new TblEmployee
                {
                    Id = emp.Id,
                    Name = emp.Name,
                    Salary = emp.Sal,
                    Location = emp.Location,
                    Deptno = emp.deptNum
                };
                db.TblEmployees.Add(tblEmployee);
                db.SaveChanges();
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string newLocation)
        {
            using (DemodbContext db = new DemodbContext())
            {
                var employee = db.TblEmployees.FirstOrDefault(e => e.Id == id);
                if (employee == null)
                {
                    return NotFound();
                }

                employee.Location = newLocation;
                db.SaveChanges();

                return NoContent();
            }
        }
    }
}

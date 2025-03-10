using EmployeeDemo.Models;
using Microsoft.AspNetCore.Mvc;
namespace EmployeeDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            List<Employee> employees;
            DemodbContext db = new DemodbContext();
            employees = db.Employees.ToList();
            return Ok(employees);
        }
    }
}

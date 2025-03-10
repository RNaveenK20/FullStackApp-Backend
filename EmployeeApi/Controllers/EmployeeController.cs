namespace EmployeeApi.Controllers
{
    using EmployeeApi.Models;
    using Microsoft.AspNetCore.Mvc;
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private static readonly List<Employee> Employees = new List<Employee>
        {
            new Employee { id = 1, name = "John Doe", position = "Software Developer" },
            new Employee { id = 2, name = "Jane Doe", position = "Project Manager" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<Employee>> GetEmployees()
        {
            return Employees;
        }

    }
}

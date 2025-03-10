using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EmployeeDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class EmployeeController1 : Controller
    {
        Employee employee = new Employee();
        [HttpGet]
        public IActionResult Get()
        {
            List<Employee> employees = new List<Employee>();
            string connectionString = @"data source=localhost; integrated security = SSPI; database=demodb; TrustServerCertificate=true;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("select * from Employee", connection);
                SqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    Employee emp = new Employee();
                    emp.num = Convert.ToInt32(dataReader["id"]);
                    emp.Name = dataReader["name"].ToString() ?? string.Empty;
                    emp.Sal = Convert.ToInt32(dataReader["salary"]);
                    emp.Location = dataReader["location"].ToString() ?? string.Empty;
                    employees.Add(emp);
                }
                //return View(employees);
            }
            return Ok(employees);
        }


        [HttpPost]
        public void Post([FromBody] Employee newEmployee)
        {
            string connectionString = @"data source=localhost; integrated security = SSPI; database=demodb; TrustServerCertificate=true;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("insert into Employee values (@id,@name,@salary,@location)", connection);
                cmd.Parameters.AddWithValue("@id", newEmployee.num);
                cmd.Parameters.AddWithValue("@name", newEmployee.Name);
                cmd.Parameters.AddWithValue("@salary", newEmployee.Sal);
                cmd.Parameters.AddWithValue("@location", newEmployee.Location);

                int result = cmd.ExecuteNonQuery();
            }
        }
        [HttpPut]
        public void Put([FromBody] Employee newEmployee)
        {
            string connectionString = @"data source=localhost; integrated security = SSPI; database=demodb; TrustServerCertificate=true;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("update Employee set name=@name,salary=@salary,location=@location where id=@id", connection);
                cmd.Parameters.AddWithValue("@id", newEmployee.num);
                cmd.Parameters.AddWithValue("@name", newEmployee.Name);
                cmd.Parameters.AddWithValue("@salary", newEmployee.Sal);
                cmd.Parameters.AddWithValue("@location", newEmployee.Location);
                int result = cmd.ExecuteNonQuery();
            }
        }
        [HttpDelete]
        public void Delete([FromBody] Employee newEmployee)
        {
            string connectionString = @"data source=localhost; integrated security = SSPI; database=demodb; TrustServerCertificate=true;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("delete from Employee where id=@id", connection);
                cmd.Parameters.AddWithValue("@id", newEmployee.num);
                int result = cmd.ExecuteNonQuery();
            }
        }
    }
}

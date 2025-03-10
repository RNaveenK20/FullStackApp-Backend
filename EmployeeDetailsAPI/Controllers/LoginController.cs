using EmployeeDetailsAPI.ClassModel;
using EmployeeDetailsAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeDetailsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Login([FromBody] LoginModel login)
        {
            DemodbContext demodbContext = new DemodbContext();
            TblEmployee employee = demodbContext.TblEmployees.FirstOrDefault(e => e.Name.ToLower() == login.userName.ToLower());
            if (employee != null)
            {
                //Generating Token Here
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", login.userName),
                    new Claim("Location", employee.Location),
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(60),
                    signingCredentials: signIn
                    );
                string jwttoken = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(jwttoken);
            }
            else
            {
                return BadRequest("Credentials are Invalid");
            }
        }
    }
}

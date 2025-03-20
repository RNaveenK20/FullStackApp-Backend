using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Model;
using Backend.DBModel;

namespace server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly RecipescrudContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(RecipescrudContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserLogin userLogin)
        {
            if (userLogin == null)
            {
                return BadRequest("Invalid user data.");
            }

            // Retrieve the user from the database
            var user = _context.TblUsers.SingleOrDefault(u => u.Email == userLogin.registeredEmailAddr);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Hash the provided password using SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(userLogin.registeredPassword));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                string hashedPassword = builder.ToString();

                Console.WriteLine($"Stored Password Hash: {user.PasswordHash}");
                Console.WriteLine($"Computed Password Hash: {hashedPassword}");

                // Compare the hashed password with the stored hashed password
                if (hashedPassword != user.PasswordHash)
                {
                    return Unauthorized("Invalid email or password.");
                }

                // Generate JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        //new Claim("UserId", user.UserId.ToString()) // Include UserId in the token

                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new { Token = tokenString });
            }
        }
    }
}
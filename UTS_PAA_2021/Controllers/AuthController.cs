using Microsoft.AspNetCore.Mvc;
using UTS_PAA_2021.Data;
using UTS_PAA_2021.Models;

namespace UTS_PAA_2021.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Username == login.Username && u.Password == login.Password);

            if (user == null)
                return Unauthorized("Invalid username or password");

            return Ok("Login successful");
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Username == user.Username);
            if (existingUser != null)
                return BadRequest("Username already exists");

            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("User registered successfully");
        }
    }
}

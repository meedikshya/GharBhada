using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GharBhada.Utils;
using GharBhada.Data;
using Microsoft.EntityFrameworkCore;

namespace GharBhada.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _tokenService;
        private readonly GharBhadaContext _context;

        public AuthController(JwtTokenService tokenService, GharBhadaContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordHash == request.PasswordHash);

            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var token = _tokenService.GenerateToken(user.UserId.ToString(), user.UserRole);
            return Ok(new { Token = token });
        }

        [Authorize]
        [HttpGet("test")]
        public IActionResult TestAuth()
        {
            return Ok("You are authenticated!");
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
using FirebaseAdmin.Auth;
using GharBhada.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace GharBhada.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _tokenService;
        private readonly GharBhadaContext _context;
        private readonly ILogger<AuthController> _logger;

        public AuthController(JwtTokenService tokenService, GharBhadaContext context, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _context = context;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Log the received email for debugging
                _logger.LogInformation("Received email: {Email}", request.Email);

                // Find the user in the database
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

                if (user == null)
                {
                    _logger.LogWarning("User not found for email: {Email}", request.Email);
                    return Unauthorized(new { Message = "User not found." });
                }

                // Log the found user ID for debugging
                _logger.LogInformation("User found with ID: {UserId}", user.UserId);

                // Generate a custom JWT token
                var token = _tokenService.GenerateToken(user.UserId.ToString(), request.Email);

                // Log the generated token for debugging
                _logger.LogInformation("Generated JWT Token: {Token}", token);

                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred: {Message}", ex.Message);
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }

    public class LoginRequest
    {
        public required string Email { get; set; }
    }
}
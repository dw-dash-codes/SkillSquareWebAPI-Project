using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace SkillSquare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var allowedRoles = new[] { "Customer", "Provider" };
            if (!allowedRoles.Contains(request.Role))
                return BadRequest("Invalid role");

            var user = new ApplicationUser
            {
                UserName = request.Email, // This is important
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Role = request.Role,
                Address = request.Address ?? "",
                Area = request.Area ?? "",
                City = request.City ?? "",
                HourlyRate = request.HourlyRate ?? 0,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = true // Dev mode only
            };

            var result = await _authRepository.RegisterAsync(user, request.Password, request.Role);

            if (result.Contains("User registered"))
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {

            var token = await _authRepository.LoginAsync(request.Email, request.Password);
            return Ok(new { Token = token });
        }


    }
}

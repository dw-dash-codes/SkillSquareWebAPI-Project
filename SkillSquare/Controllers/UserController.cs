using System.Security.Claims;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace SkillSquare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepostory;

        public UserController(IUserRepository userRepository)
        {
            _userRepostory = userRepository;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var profile = await _userRepostory.GetProfileAsync(userId);

            if (profile == null) { return NotFound("User Not Found"); }

            return Ok(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateUserProfileRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var success = await _userRepostory.UpdateProfileAsync(userId,request);

            if (!success)
                return BadRequest("Profile update failed");

            return Ok("Profile updated successfully");

        }

    }
}

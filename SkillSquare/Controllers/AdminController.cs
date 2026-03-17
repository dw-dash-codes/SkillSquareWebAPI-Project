using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SkillSquare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepo;

        public AdminController(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        [HttpGet("dashboard-stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = await _adminRepo.GetDashboardStatsAsync();
            return Ok(stats);
        }

        [HttpGet("bookings")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _adminRepo.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpPut("bookings/{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            var success = await _adminRepo.UpdateBookingStatusAsync(id, status);
            if (!success) return BadRequest("Could not update booking status");
            return Ok($"Booking status updated to {status}");
        }

        [HttpGet("providers")]
        public async Task<IActionResult> GetAllProviders()
        {
            var providers = await _adminRepo.GetAllProvidersAsync();
            return Ok(providers);
        }

        // Update Provider Status (Approve/Suspend)
        [HttpPut("providers/{id}/status")]
        public async Task<IActionResult> UpdateProviderStatus(string id, [FromQuery] bool isActive)
        {
            var success = await _adminRepo.UpdateProviderStatusAsync(id, isActive);
            if (!success) return BadRequest("Could not update provider status");
            return Ok(new { Message = "Provider status updated successfully" });
        }


        // Get All Reviews
        [HttpGet("reviews")]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _adminRepo.GetAllReviewsAsync();
            return Ok(reviews);
        }

        // Delete Review
        [HttpDelete("reviews/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var success = await _adminRepo.DeleteReviewAsync(id);
            if (!success) return BadRequest("Could not delete review (not found)");
            return Ok(new { Message = "Review deleted successfully" });
        }


        // Get All Customers
        [HttpGet("customers")]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _adminRepo.GetAllCustomersAsync();
            return Ok(customers);
        }

        // Block or Unblock User (Works for Customers too)
        [HttpPut("users/{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(string id, [FromQuery] bool isActive)
        {
            var success = await _adminRepo.UpdateUserStatusAsync(id, isActive);
            if (!success) return BadRequest("Could not update user status");

            var statusMsg = isActive ? "activated" : "blocked";
            return Ok(new { Message = $"User {statusMsg} successfully" });
        }

    }
}
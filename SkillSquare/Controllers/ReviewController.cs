using System.Security.Claims;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace SkillSquare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer,Provider,Admin")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _repo;
        private readonly IBookingRepository _bookingRepo;

        public ReviewController(IReviewRepository repo, IBookingRepository bookingRepo)
        {
            _repo = repo;
            _bookingRepo = bookingRepo;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Add([FromBody] ReviewRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var booking = await _bookingRepo.GetBookingByIdAsync(request.BookingId);
            if (booking == null || booking.CustomerId != customerId)
                return BadRequest("Booking not found or not yours.");

            if (booking.Status != "Completed")
                return BadRequest("You can only review completed bookings.");

            // 🚨 SECURITY FIX: Prevent duplicate reviews for the same booking
            var existingReviews = await _repo.GetReviewsByCustomerAsync(customerId);
            if (existingReviews.Any(r => r.BookingId == request.BookingId))
            {
                return BadRequest("You have already reviewed this booking.");
            }

            var review = new Review
            {
                BookingId = request.BookingId,
                CustomerId = customerId,
                ProviderId = booking.ProviderId,
                Rating = request.Rating,
                Comment = request.Comment
            };

            var ok = await _repo.AddReviewAsync(review);
            if (!ok) return BadRequest("Could not add review");

            return Ok(new { Message = "Review added successfully" });
        }

        [HttpGet("provider/{providerId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetForProvider(string providerId)
        {
            var list = await _repo.GetReviewsForProviderAsync(providerId);
            return Ok(list);
        }

        [HttpGet("my")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyReviews()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _repo.GetReviewsByCustomerAsync(customerId);
            return Ok(list);
        }

        [HttpGet("average/{providerId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAverageRating(string providerId)
        {
            var avgRating = await _repo.GetAverageRatingForProviderAsync(providerId);
            return Ok(new { ProviderId = providerId, AverageRating = avgRating });
        }
    }
}


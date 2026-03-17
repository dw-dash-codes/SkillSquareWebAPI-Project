using System.Security.Claims;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;

namespace SkillSquare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _repo;
        private readonly INotificationRepository _notifRepo;
        private readonly IHubContext<NotificationHub> _hub;

        public BookingController(
            IBookingRepository repo,
            INotificationRepository notifRepo,
            IHubContext<NotificationHub> hub)
        {
            _repo = repo;
            _notifRepo = notifRepo;
            _hub = hub;
        }

        // 🔔 Helper: Send notification (DB + SignalR)
        private async Task SendNotification(string userId, string message, string type, int? bookingId = null)
        {
            var notif = new Notification
            {
                UserId = userId,  // AspNetUsers.Id
                Message = message,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                Type = type,           
                BookingId = bookingId,
            };

            await _notifRepo.AddNotificationAsync(notif);
            await _hub.Clients.Group(userId).SendAsync("ReceiveNotification", message);
        }

        
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([FromBody] BookingRequest request)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var booking = new Booking
            {
                CustomerId = customerId,
                ProviderId = request.ProviderId,
                BookingDate = request.BookingDate,
                BookingTime = request.BookingTime,
                CustomerAddress = request.CustomerAddress,
                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                Description = request.Description,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            var ok = await _repo.CreateBookingAsync(booking);
            if (!ok) return BadRequest("Could not create booking.");

            // Notify provider
            await SendNotification(request.ProviderId,"You have a new booking request.","BookingRequest",booking.Id);

            return Ok(new { Message = "Booking request sent.", BookingId = booking.Id });
        }

        
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] string status)
        {
            var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ok = await _repo.UpdateBookingStatusAsync(id, providerId, status);

            if (!ok) return BadRequest("Could not update status (invalid booking or status).");

            var booking = (await _repo.GetBookingsForProviderAsync(providerId))
                            .FirstOrDefault(b => b.Id == id);

            if (booking != null)
            {
                if (status == "Accepted")
                    await SendNotification(booking.CustomerId, "Your booking was accepted.", "BookingAccepted", booking.Id);
                else if (status == "Rejected")
                    await SendNotification(booking.CustomerId, "Your booking was rejected.", "BookingRejected", booking.Id);
                else if (status == "Completed")
                    await SendNotification(booking.CustomerId, "Booking completed. Please leave a review.", "BookingCompleted", booking.Id);
            }

            return Ok("Booking status updated.");
        }

        
        [HttpGet("user/myBookings")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyBookings()
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _repo.GetBookingsForCustomerAsync(customerId);
            return Ok(list);
        }

        
        [HttpGet("provider/myBookings")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> ProviderBookings()
        {
            var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _repo.GetBookingsForProviderAsync(providerId);
            return Ok(list);
        }

        
        [HttpPut("{id}/complete")]
        [Authorize(Roles = "Provider")]
        public async Task<IActionResult> CompleteBooking(int id)
        {
            var providerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ok = await _repo.UpdateBookingStatusAsync(id, providerId, "Completed");

            if (!ok) return BadRequest("Could not mark as completed.");

            var booking = (await _repo.GetBookingsForProviderAsync(providerId))
                            .FirstOrDefault(b => b.Id == id);

            if (booking != null)
            {
                await SendNotification(booking.CustomerId, "Booking marked as completed. Please leave a review.","BookingCompleted",booking.Id);
            }

            return Ok("Booking marked as completed.");
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _repo.GetBookingByIdAsync(id);

            if (booking == null)
                return NotFound("Booking not found");

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Security check: Only the customer or provider involved can see these details
            if (booking.CustomerId != currentUserId && booking.ProviderId != currentUserId)
                return Forbid("You do not have permission to view this booking.");

            return Ok(new
            {
                Id = booking.Id,
                ProviderName = $"{booking.Provider.FirstName} {booking.Provider.LastName}",
                ServiceCategory = booking.Provider.Category != null ? booking.Provider.Category.Title : "Service",
                TaskDescription = booking.Description ?? "No description provided",
                Date = booking.BookingDate.ToString("MMM dd, yyyy")
            });
        }
    }
}

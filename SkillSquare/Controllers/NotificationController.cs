using System.Security.Claims;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;

namespace SkillSquare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _repo;
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationController(INotificationRepository repo, IHubContext<NotificationHub> hub)
        {
            _hub = hub;
            _repo = repo;
        }

        [HttpGet("myNotifications")]
        public async Task<IActionResult> MyNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var list = await _repo.GetNotificationsForUserAsync(userId);
            return Ok(list);
        }

        
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ok = await _repo.MarkAsReadAsync(id, userId);
            if (!ok) return BadRequest("Notification not found.");
            return Ok("Notification marked as read.");
        }

        private async Task SendNotification(string userId, string message, string type = "System", int? bookingId = null)
        {
            var notif = new Notification
            {
                UserId = userId,
                Message = message,
                Type = type,
                BookingId = bookingId
            };
            await _repo.AddNotificationAsync(notif);

            // Frontend ko pura object bhejo
            await _hub.Clients.Group(userId).SendAsync("ReceiveNotification", notif);
        }

    }
}

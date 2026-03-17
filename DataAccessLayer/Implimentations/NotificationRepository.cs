using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer.Implimentations
{
    public class NotificationRepository : INotificationRepository
    {

        private readonly AppDbContext _ctx;
        public NotificationRepository(AppDbContext ctx) { _ctx = ctx; }

        public async Task<bool> AddNotificationAsync(Notification notification)
        {
            _ctx.Notifications.Add(notification);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsForUserAsync(string userId)
        {
            return await _ctx.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> MarkAsReadAsync(int id, string userId)
        {
            var notif = await _ctx.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);
            if (notif == null) return false;

            notif.IsRead = true;
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}

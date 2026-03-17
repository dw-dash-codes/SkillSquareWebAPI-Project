using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAccessLayer.Interfaces
{
    public interface INotificationRepository
    {
        Task<bool> AddNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetNotificationsForUserAsync(string userId);
        Task<bool> MarkAsReadAsync(int id, string userId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string UserId { get; set; }   
        public ApplicationUser User { get; set; }

        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Type { get; set; }  
        public int? BookingId { get; set; }
    }
}

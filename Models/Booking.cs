using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Booking
    {
        public int Id { get; set; }

        // relationships
        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }

        public string ProviderId { get; set; }
        public ApplicationUser Provider { get; set; }

        // details
        public DateTime BookingDate { get; set; }
        public string BookingTime { get; set; }  // "09:00", "14:30" etc.
        public string CustomerAddress { get; set; }
        public string Description { get; set; }

        // lifecycle
        public string Status { get; set; } = "Pending"; // Pending | Accepted | Rejected | Completed

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

    }
}

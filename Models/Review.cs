using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Review
    {
        public int Id { get; set; }

        // relationships
        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }

        public string ProviderId { get; set; }
        public ApplicationUser Provider { get; set; }

        // details
        public int Rating { get; set; } // 1–5
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

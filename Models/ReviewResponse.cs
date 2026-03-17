using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ReviewResponse
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string CustomerName { get; set; }
        public string ProviderName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

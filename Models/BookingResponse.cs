using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BookingResponse
    {
        public int Id { get; set; }
        public string ProviderId { get; set; }
        public string CustomerId { get; set; }
        public string ProviderName { get; set; }
        public string CustomerName { get; set; }

        public string ServiceCategory { get; set; }

        public DateTime BookingDate { get; set; }
        public string BookingTime { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string Description { get; set; }
    }
}

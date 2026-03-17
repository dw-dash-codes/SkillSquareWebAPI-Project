using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BookingRequest
    {
        public string ProviderId { get; set; }
        public DateTime BookingDate { get; set; }
        public string BookingTime { get; set; }
        
        public string? Description { get; set; }

        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
    }
}

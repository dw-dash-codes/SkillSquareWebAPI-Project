using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProviderAdminDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CategoryName { get; set; }
        public string City { get; set; }
        public bool IsActive { get; set; } // To track Approved/Suspended status
        public decimal HourlyRate { get; set; }
    }
}

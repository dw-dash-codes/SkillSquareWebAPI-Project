using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProviderUpdateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public int CategoryId { get; set; }
        public string Skills { get; set; }
        public string Bio { get; set; }
        public decimal HourlyRate { get; set; }

    }
}

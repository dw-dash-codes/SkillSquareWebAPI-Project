using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserProfileResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string? Address { get; set; }
        public string? Area { get; set; }
        public string? City { get; set; }

        public string? PhoneNumber { get; set; }
    }
}

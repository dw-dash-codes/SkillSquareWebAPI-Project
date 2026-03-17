using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role {  get; set; }
        public string? Address {  get; set; }
        public string? Area {  get; set; }
        public string? City { get; set; }
        public bool isActive { get; set; } = true;

        public int? CategoryId { get; set; }
        public string? Skills { get; set; }
        public string? Bio { get; set; }

        public decimal HourlyRate { get; set; }

        
        public ServiceCategory? Category { get; set; }
    }
}

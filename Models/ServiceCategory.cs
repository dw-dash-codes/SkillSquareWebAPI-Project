using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ServiceCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }   
        public string? IconClass { get; set; } 

        public ICollection<ApplicationUser> Providers { get; set; }
    }
}

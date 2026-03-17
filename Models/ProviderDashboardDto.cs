using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProviderDashboardDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string City { get; set; }
        public string CategoryName { get; set; }
        public decimal HourlyRate { get; set; }
    }
}

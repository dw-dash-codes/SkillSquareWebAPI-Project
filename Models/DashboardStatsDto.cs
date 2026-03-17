using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalProviders { get; set; }
        public int TotalBookings { get; set; }
        public int PendingReviews { get; set; }

        public List<ProviderDashboardDto> RecentProviders { get; set; }
    }
}

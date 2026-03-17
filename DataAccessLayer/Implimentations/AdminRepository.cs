using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer.Implementations
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _ctx;

        public AdminRepository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            // 1. Get Counts (Keep this same)
            var totalCustomers = await _ctx.Users.CountAsync(u => u.Role == "Customer");
            var totalProviders = await _ctx.Users.CountAsync(u => u.Role == "Provider");
            var totalBookings = await _ctx.Bookings.CountAsync();
            var totalReviews = await _ctx.Reviews.CountAsync();

            // 2. Get Recent Providers (FIXED QUERY)
            var recentProviders = await _ctx.Users
                .Where(u => u.Role == "Provider")
                .OrderByDescending(u => u.Id)
                .Take(5)
                .Select(u => new ProviderDashboardDto
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName,
                    City = u.City ?? "Unknown",
                    CategoryName = u.Category != null ? u.Category.Title : "Uncategorized",
                    HourlyRate = u.HourlyRate
                })
                .ToListAsync();

            return new DashboardStatsDto
            {
                TotalUsers = totalCustomers,
                TotalProviders = totalProviders,
                TotalBookings = totalBookings,
                PendingReviews = totalReviews,
                RecentProviders = recentProviders
            };
        }
        public async Task<IEnumerable<BookingResponse>> GetAllBookingsAsync()
        {
            return await _ctx.Bookings
                .AsNoTracking()
                .Include(b => b.Customer)
                .Include(b => b.Provider)
                .ThenInclude(p => p.Category) // Join Provider -> Category
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookingResponse
                {
                    Id = b.Id,
                    CustomerId = b.CustomerId,
                    CustomerName = b.Customer.FirstName + " " + b.Customer.LastName,
                    ProviderId = b.ProviderId,
                    ProviderName = b.Provider.FirstName + " " + b.Provider.LastName,

                    // ✅ Map the category title
                    ServiceCategory = b.Provider.Category != null ? b.Provider.Category.Title : "General",

                    BookingDate = b.BookingDate,
                    BookingTime = b.BookingTime,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt,
                    CustomerPhone = b.CustomerPhone
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateBookingStatusAsync(int bookingId, string status)
        {
            var booking = await _ctx.Bookings.FindAsync(bookingId);
            if (booking == null) return false;

            booking.Status = status;
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ProviderAdminDto>> GetAllProvidersAsync()
        {
            return await _ctx.Users
                .Where(u => u.Role == "Provider")
                .Include(u => u.Category)
                .OrderByDescending(u => u.Id)
                .Select(u => new ProviderAdminDto
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    CategoryName = u.Category != null ? u.Category.Title : "Uncategorized",
                    City = u.City,
                    IsActive = u.isActive,
                    HourlyRate = u.HourlyRate
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateProviderStatusAsync(string providerId, bool isActive)
        {
            //var user = await _ctx.Users.FindByAsync(providerId); // or .FindAsync(providerId) if using DbContext directly
                                                                   // Since _ctx is AppDbContext, use:
                                                                   // var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == providerId);

            // Better approach if you injected UserManager, but using Context is fine for simple updates:
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == providerId);

            if (user == null) return false;

            user.isActive = isActive;
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ReviewAdminDto>> GetAllReviewsAsync()
        {
            return await _ctx.Reviews
                .AsNoTracking()
                .Include(r => r.Customer)
                .Include(r => r.Provider)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewAdminDto
                {
                    Id = r.Id,
                    CustomerName = r.Customer.FirstName + " " + r.Customer.LastName,
                    ProviderName = r.Provider.FirstName + " " + r.Provider.LastName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await _ctx.Reviews.FindAsync(reviewId);
            if (review == null) return false;

            _ctx.Reviews.Remove(review);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<UserAdminDto>> GetAllCustomersAsync()
        {
            return await _ctx.Users
                .AsNoTracking()
                .Where(u => u.Role == "Customer") // Fetch only Customers
                .OrderByDescending(u => u.Id) // or CreatedAt if you have it
                .Select(u => new UserAdminDto
                {
                    Id = u.Id,
                    FullName = u.FirstName + " " + u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    City = u.City,
                    IsActive = u.isActive
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateUserStatusAsync(string userId, bool isActive)
        {
            var user = await _ctx.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            user.isActive = isActive;
            return await _ctx.SaveChangesAsync() > 0;
        }
    }
}
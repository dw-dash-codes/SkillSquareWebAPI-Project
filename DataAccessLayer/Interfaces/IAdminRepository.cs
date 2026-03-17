using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IAdminRepository
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();
        Task<IEnumerable<BookingResponse>> GetAllBookingsAsync();
        Task<bool> UpdateBookingStatusAsync(int bookingId, string status);

        Task<IEnumerable<ProviderAdminDto>> GetAllProvidersAsync();
        Task<bool> UpdateProviderStatusAsync(string providerId, bool isActive);

        Task<IEnumerable<ReviewAdminDto>> GetAllReviewsAsync();
        Task<bool> DeleteReviewAsync(int reviewId);

        Task<IEnumerable<UserAdminDto>> GetAllCustomersAsync();
        Task<bool> UpdateUserStatusAsync(string userId, bool isActive);
    }
}

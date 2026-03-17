using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAccessLayer.Interfaces
{
    public interface IBookingRepository
    {
        Task<bool> CreateBookingAsync(Booking booking);
        Task<IEnumerable<BookingResponse>> GetBookingsForCustomerAsync(string customerId);
        Task<IEnumerable<BookingResponse>> GetBookingsForProviderAsync(string providerId);
        Task<bool> UpdateBookingStatusAsync(int bookingId, string providerId, string status);

        Task<Booking?> GetBookingByIdAsync(int bookingId);
    }
}

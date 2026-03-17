using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer.Implimentations
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _ctx;
        public BookingRepository(AppDbContext ctx) { _ctx = ctx; }

        public async Task<bool> CreateBookingAsync(Booking booking)
        {

            _ctx.Bookings.Add(booking);
            return await _ctx.SaveChangesAsync() > 0;
            
        }

        public async Task<IEnumerable<BookingResponse>> GetBookingsForCustomerAsync(string customerId)
        {
            return await _ctx.Bookings
                .AsNoTracking()
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookingResponse
                {
                    Id = b.Id,
                    CustomerId = b.CustomerId,
                    CustomerName = b.Customer.FirstName + " " + b.Customer.LastName,
                    ProviderId = b.ProviderId,
                    ProviderName = b.Provider.FirstName + " " + b.Provider.LastName,
                    BookingDate = b.BookingDate,
                    BookingTime = b.BookingTime,
                    CustomerAddress = b.CustomerAddress,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt,
                    CustomerPhone = b.CustomerPhone,
                    Description = b.Description,
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<BookingResponse>> GetBookingsForProviderAsync(string providerId)
        {
            return await _ctx.Bookings
                .AsNoTracking()
                .Where(b => b.ProviderId == providerId)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new BookingResponse
                {
                    Id = b.Id,
                    CustomerId = b.CustomerId,
                    CustomerName = b.Customer.FirstName + " " + b.Customer.LastName,
                    ProviderId = b.ProviderId,
                    ProviderName = b.Provider.FirstName + " " + b.Provider.LastName,
                    BookingDate = b.BookingDate,
                    BookingTime = b.BookingTime,
                    CustomerAddress = b.CustomerAddress,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt,
                    CustomerPhone = b.CustomerPhone
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateBookingStatusAsync(int bookingId, string providerId, string status)
        {
            var booking = await _ctx.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId && b.ProviderId == providerId);
            if (booking == null) return false;

            status = status?.Trim();
            if (status != "Accepted" && status != "Rejected" && status != "Pending" && status != "Completed")
                return false;

            booking.Status = status;
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _ctx.Bookings
                .Include(b => b.Provider)
                .Include(b => b.Customer)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }
    }
}

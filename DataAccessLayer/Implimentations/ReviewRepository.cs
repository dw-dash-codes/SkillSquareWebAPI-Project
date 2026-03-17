using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer.Implimentations
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _ctx;
        public ReviewRepository(AppDbContext ctx) { _ctx = ctx; }

        public async Task<bool> AddReviewAsync(Review review)
        {
            _ctx.Reviews.Add(review);
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ReviewResponse>> GetReviewsForProviderAsync(string providerId)
        {
            return await _ctx.Reviews
                .AsNoTracking()
                .Where(r => r.ProviderId == providerId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewResponse
                {
                    Id = r.Id,
                    BookingId = r.BookingId,
                    CustomerName = r.Customer.FirstName + " " + r.Customer.LastName,
                    ProviderName = r.Provider.FirstName + " " + r.Provider.LastName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ReviewResponse>> GetReviewsByCustomerAsync(string customerId)
        {
            return await _ctx.Reviews
                .AsNoTracking()
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewResponse
                {
                    Id = r.Id,
                    BookingId = r.BookingId,
                    CustomerName = r.Customer.FirstName + " " + r.Customer.LastName,
                    ProviderName = r.Provider.FirstName + " " + r.Provider.LastName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingForProviderAsync(string providerId)
        {
            // This tells SQL Server to calculate the average directly
            var average = await _ctx.Reviews
                .Where(r => r.ProviderId == providerId)
                .AverageAsync(r => (double?)r.Rating);

            if (average == null)
                return 0;

            return Math.Round(average.Value, 1);
        }
    }
}

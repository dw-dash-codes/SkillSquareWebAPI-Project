using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAccessLayer.Interfaces
{
    public interface IReviewRepository
    {
        Task<bool> AddReviewAsync(Review review);
        Task<IEnumerable<ReviewResponse>> GetReviewsForProviderAsync(string providerId);
        Task<IEnumerable<ReviewResponse>> GetReviewsByCustomerAsync(string customerId);
        Task<double> GetAverageRatingForProviderAsync(string providerId);

    }
}

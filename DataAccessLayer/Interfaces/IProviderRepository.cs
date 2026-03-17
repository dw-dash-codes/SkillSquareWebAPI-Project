using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAccessLayer.Interfaces
{
    public interface IProviderRepository
    {
        Task<IEnumerable<ApplicationUser>> GetProvidersByCategoryAsync(int catgeoryId);
        Task<bool> RegisterProviderAsync(ApplicationUser provider, string password);
        Task<IEnumerable<ApplicationUser>> GetAllProvidersAsync();
        Task<ApplicationUser?> GetProviderByIdAsync(string id);
        Task<bool> UpdateProviderAsync(ApplicationUser provider);
        Task<bool> DeleteProviderAsync(string id);

        Task<IEnumerable<ApplicationUser>> SearchProvidersAsync(string query);

    }
}

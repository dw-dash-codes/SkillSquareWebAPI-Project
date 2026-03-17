using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAccessLayer.Interfaces
{
    public interface IServiceCategoryRepository
    {
        Task<IEnumerable<ServiceCategory>> GetAllAsync();
        Task<ServiceCategory?> GetByIdAsync(int id);
        Task<bool> CreateAsync(ServiceCategory category);
        Task<bool> UpdateAsync(ServiceCategory category);
        Task<bool> DeleteAsync(int id);




    }
}

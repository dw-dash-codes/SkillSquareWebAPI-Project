using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer.Implimentations
{
    public class ProviderRepository : IProviderRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProviderRepository( AppDbContext context , UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IEnumerable<ApplicationUser>> GetProvidersByCategoryAsync(int catgeoryId)
        {
            return await _context.Users
                .Where(u => u.Role == "Provider" && u.CategoryId == catgeoryId && u.isActive)
                .ToListAsync();
        }

        public async Task<bool> RegisterProviderAsync(ApplicationUser provider, string password)
        {
            var result = await _userManager.CreateAsync(provider, password);
            if (!result.Succeeded) return false;

            await _userManager.AddToRoleAsync(provider, "Provider");
            return true;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllProvidersAsync()
        {
            return await Task.FromResult(_userManager.Users
                .Include(p => p.Category)
                
                .Where(u => u.Role == "Provider").ToList());
        }

        public async Task<ApplicationUser?> GetProviderByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> UpdateProviderAsync(ApplicationUser provider)
        {
            var result = await _userManager.UpdateAsync(provider);
            return result.Succeeded;
        }

        public async Task<bool> DeleteProviderAsync(string id)
        {
            var provider = await _userManager.FindByIdAsync(id);
            if (provider == null) return false;

            var result = await _userManager.DeleteAsync(provider);
            return result.Succeeded;
        }

        public async Task<IEnumerable<ApplicationUser>> SearchProvidersAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<ApplicationUser>();
            }
            query = query.ToLower();

            return await _userManager.Users
                .Include(u => u.Category) // Category table join karo
                .Where(u =>
                    u.Role == "Provider" && // Sirf Providers hon
                    u.isActive == true &&   // Active/Not Deleted hon
                    (
                        u.FirstName.ToLower().Contains(query) ||
                        u.LastName.ToLower().Contains(query) ||
                        u.City.ToLower().Contains(query) ||
                        u.Bio.ToLower().Contains(query) ||
                        (u.Category != null && u.Category.Title.ToLower().Contains(query))
                    )
                )
                .ToListAsync();
        }
    }
}

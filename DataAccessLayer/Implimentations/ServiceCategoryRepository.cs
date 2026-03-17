using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Data;
using DataAccessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccessLayer.Implimentations
{
    public class ServiceCategoryRepository : IServiceCategoryRepository
    {
        private readonly AppDbContext _context;

        public ServiceCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServiceCategory>> GetAllAsync()
        {
            return await _context.ServiceCategories.ToListAsync();
        }

        public async Task<ServiceCategory?> GetByIdAsync(int id)
        {
            return await _context.ServiceCategories.FindAsync(id);
        }

        public async Task<bool> CreateAsync(ServiceCategory category)
        {
            _context.ServiceCategories.Add(category);
            return await _context.SaveChangesAsync() > 0;  
        }

        public async Task<bool> UpdateAsync(ServiceCategory category)
        {
            _context.ServiceCategories.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.ServiceCategories.FindAsync(id);
            if (category == null) return false;
            _context.ServiceCategories.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

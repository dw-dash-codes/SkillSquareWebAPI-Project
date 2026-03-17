using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using Models;

namespace DataAccessLayer.Implimentations
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository( UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager; 
        }
        public async Task<UserProfileResponse> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new UserProfileResponse
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                Address = user.Address,
                Area = user.Area,
                City = user.City,
                PhoneNumber = user.PhoneNumber
            };

        }

        public async Task<bool> UpdateProfileAsync(string userId, UpdateUserProfileRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Address = request.Address;
            user.Area = request.Area;
            user.City = request.City;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}

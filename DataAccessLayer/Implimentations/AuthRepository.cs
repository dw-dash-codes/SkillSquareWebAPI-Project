using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Utilities;
using Microsoft.AspNetCore.Identity;
using Models;

namespace DataAccessLayer.Implimentations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<string> RegisterAsync(ApplicationUser user, string password, string role)
        {
            var createUser = await _userManager.CreateAsync(user, password);
            if (!createUser.Succeeded)
                return string.Join(", ", createUser.Errors.Select(e => e.Description));

            await _userManager.AddToRoleAsync(user, role);
            return "User registered successfully.";
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return "Invalid Email";

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded)
            { return "Invalid Credentails"; }

            return _jwtTokenGenerator.GenerateToken(user);
        }



    }
}

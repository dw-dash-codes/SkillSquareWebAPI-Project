using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAccessLayer.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> RegisterAsync(ApplicationUser user, string password, string role);
        Task<string> LoginAsync(string email, string password);

    }
}

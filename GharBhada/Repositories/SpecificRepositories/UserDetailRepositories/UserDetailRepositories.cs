using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GharBhada.Models;
using GharBhada.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;

namespace GharBhada.Repositories.SpecificRepositories.UserDetailRepositories
{
    public class UserDetailRepositories : IUserDetailRepositories
    {
        private readonly IGenericRepositories _genericRepositories;

        public UserDetailRepositories(IGenericRepositories genericRepositories)
        {
            _genericRepositories = genericRepositories;
        }

        public async Task<IEnumerable<UserDetail>> GetUserDetailsByUserId(int userId)
        {
            return await _genericRepositories.SelectAll<UserDetail>(u => u.UserId == userId);
        }
    }
}
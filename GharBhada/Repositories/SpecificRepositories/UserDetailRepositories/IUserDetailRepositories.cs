using System.Collections.Generic;
using System.Threading.Tasks;
using GharBhada.Models;

namespace GharBhada.Repositories.SpecificRepositories.UserDetailRepositories
{
    public interface IUserDetailRepositories
    {
        Task<IEnumerable<UserDetail>> GetUserDetailsByUserId(int userId);
    }
}
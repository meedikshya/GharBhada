using System.Linq;
using System.Threading.Tasks;
using GharBhada.Models;
using GharBhada.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;

namespace GharBhada.Repositories.SpecificRepositories.UserRepositories
{
    public class UserRepositories : IUserRepositories
    {
        private readonly IGenericRepositories _genericRepositories;

        public UserRepositories(IGenericRepositories genericRepositories)
        {
            _genericRepositories = genericRepositories;
        }

        public async Task<int> GetUserIdByFirebaseId(string firebaseUserId)
        {
            var users = await _genericRepositories.SelectAll<User>(u => u.FirebaseUId == firebaseUserId);
            return users.Select(u => u.UserId).FirstOrDefault();
        }

        public async Task<string> GetFirebaseUserIdByUserId(int userId)
        {
            var users = await _genericRepositories.SelectAll<User>(u => u.UserId == userId);
            return users.Select(u => u.FirebaseUId).FirstOrDefault();
        }
    }
}
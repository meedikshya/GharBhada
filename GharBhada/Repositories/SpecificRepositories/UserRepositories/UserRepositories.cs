using System.Linq;
using System.Threading.Tasks;
using GharBhada.Data;
using GharBhada.Models;
using GharBhada.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;

namespace GharBhada.Repositories.SpecificRepositories.UserRepositories
{
    public class UserRepositories : IUserRepositories
    {
        private readonly IGenericRepositories _genericRepositories;
        private readonly GharBhadaContext _context;

        public UserRepositories(IGenericRepositories genericRepositories, GharBhadaContext context)
        {
            _genericRepositories = genericRepositories;
            _context = context;
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

        public async Task<int> GetTotalUserCount()
        {
            return await _context.Set<User>().CountAsync();
        }

        public async Task<int> GetTotalLandlordCount()
        {
            return await _context.Set<User>().CountAsync(u => u.UserRole == "landlord");
        }

        public async Task<int> GetTotalRenterCount()
        {
            return await _context.Set<User>().CountAsync(u => u.UserRole == "renter");
        }
    }
}
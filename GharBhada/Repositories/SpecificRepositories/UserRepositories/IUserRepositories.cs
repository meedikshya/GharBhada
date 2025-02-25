using System.Threading.Tasks;
using GharBhada.Models;

namespace GharBhada.Repositories.SpecificRepositories.UserRepositories
{
    public interface IUserRepositories
    {
        Task<int> GetUserIdByFirebaseId(string firebaseUserId);
        Task<string> GetFirebaseUserIdByUserId(int userId);
    }
}
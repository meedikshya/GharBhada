using System.Threading.Tasks;
using GharBhada.Models;

namespace GharBhada.Repositories.SpecificRepositories.FavouriteRepositories
{
    public interface IFavouriteRepositories
    {
        Task<Favourite> GetFavouriteByUserIdAndPropertyIdAsync(int userId, int propertyId);
        Task RemoveFavouriteAsync(Favourite favourite);
    }
}
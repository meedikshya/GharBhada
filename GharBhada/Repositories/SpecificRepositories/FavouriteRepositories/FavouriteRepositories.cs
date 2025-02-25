using System.Linq;
using System.Threading.Tasks;
using GharBhada.Models;
using GharBhada.Data;
using Microsoft.EntityFrameworkCore;
using Google;

namespace GharBhada.Repositories.SpecificRepositories.FavouriteRepositories
{
    public class FavouriteRepositories : IFavouriteRepositories
    {
        private readonly GharBhadaContext _context;

        public FavouriteRepositories(GharBhadaContext context)
        {
            _context = context;
        }

        public async Task<Favourite> GetFavouriteByUserIdAndPropertyIdAsync(int userId, int propertyId)
        {
            return await _context.Favourites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.PropertyId == propertyId);
        }

        public async Task RemoveFavouriteAsync(Favourite favourite)
        {
            _context.Favourites.Remove(favourite);
            await _context.SaveChangesAsync();
        }
    }
}
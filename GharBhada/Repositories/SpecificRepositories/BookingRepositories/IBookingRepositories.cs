using GharBhada.Models;
using System.Threading.Tasks;

namespace GharBhada.Repositories.SpecificRepositories.BookingRepositories
{
    public interface IBookingRepositories
    {
        Task<List<Booking>> GetBookingsByUserId(int userId);
        Task<int> GetAcceptedBookingCountAsync();
        Task<int> GetTotalBookingCountAsync();
    }
}
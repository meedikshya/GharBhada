using GharBhada.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GharBhada.Repositories.SpecificRepositories.BookingRepositories
{
    public interface IBookingRepositories
    {
        Task<List<Booking>> GetBookingsByUserId(int userId);
        Task<int> GetAcceptedBookingCountAsync();
        Task<int> GetTotalBookingCountAsync();
        Task<List<Booking>> GetBookingsByLandlordIdAsync(int landlordId);
    }
}
using GharBhada.Models;

namespace GharBhada.Repositories.SpecificRepositories.BookingRepositories
{
    public interface IBookingRepositories
    {
        Task<List<Booking>> GetBookingsByUserId(int userId);
    }
}

using GharBhada.Data;
using GharBhada.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GharBhada.Repositories.SpecificRepositories.BookingRepositories
{
    public class BookingRepositories : IBookingRepositories
    {
        private readonly GharBhadaContext _context;

        public BookingRepositories(GharBhadaContext context)
        {
            _context = context;
        }

        public async Task<List<Booking>> GetBookingsByUserId(int userId)
        {
            return await _context.Bookings.Where(b => b.UserId == userId).ToListAsync();
        }

        public async Task<int> GetAcceptedBookingCountAsync()
        {
            return await _context.Bookings.CountAsync(b => b.Status == "Accepted");
        }

        public async Task<int> GetTotalBookingCountAsync()
        {
            return await _context.Bookings.CountAsync();
        }
    }
}
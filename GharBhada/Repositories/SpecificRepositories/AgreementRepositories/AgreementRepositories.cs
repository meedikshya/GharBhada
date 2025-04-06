using GharBhada.Data;
using GharBhada.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GharBhada.Repositories.SpecificRepositories.AgreementRepositories
{
    public class AgreementRepositories : IAgreementRepositories
    {
        private readonly GharBhadaContext _context;

        public AgreementRepositories(GharBhadaContext context)
        {
            _context = context;
        }

        public async Task<Agreement> GetAgreementByBookingIdAsync(int bookingId)
        {
            return await _context.Agreements.FirstOrDefaultAsync(a => a.BookingId == bookingId);
        }

        public async Task<List<Agreement>> GetAgreementsByUserIdAsync(int userId)
        {
            return await _context.Agreements.Where(a => a.RenterId == userId).ToListAsync();
        }

        public async Task<List<Agreement>> GetAgreementsByLandlordIdAsync(int landlordId)
        {
            return await _context.Agreements.Where(a => a.LandlordId == landlordId).ToListAsync();
        }

        public async Task<int> GetTotalAgreementCountAsync()
        {
            return await _context.Agreements.CountAsync();
        }

        public async Task<int> GetApprovedAgreementCountAsync()
        {
            return await _context.Agreements.CountAsync(a => a.Status == "Approved");
        }
    }
}
using System.Linq;
using System.Threading.Tasks;
using GharBhada.Models;
using GharBhada.Repositories.GenericRepositories;
using Microsoft.EntityFrameworkCore;

namespace GharBhada.Repositories.SpecificRepositories.AgreementRepositories
{
    public class AgreementRepositories : IAgreementRepositories
    {
        private readonly IGenericRepositories _genericRepositories;

        public AgreementRepositories(IGenericRepositories genericRepositories)
        {
            _genericRepositories = genericRepositories;
        }

        public async Task<Agreement> GetAgreementByBookingIdAsync(int bookingId)
        {
            var agreements = await _genericRepositories.SelectAll<Agreement>(a => a.BookingId == bookingId);
            return agreements.FirstOrDefault();
        }


    }
}
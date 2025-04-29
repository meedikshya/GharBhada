using System.Collections.Generic;
using System.Threading.Tasks;
using GharBhada.Models;

namespace GharBhada.Repositories.SpecificRepositories.AgreementRepositories
{
    public interface IAgreementRepositories
    {
        Task<Agreement> GetAgreementByBookingIdAsync(int bookingId);
        Task<List<Agreement>> GetAgreementsByUserIdAsync(int userId);
        Task<List<Agreement>> GetAgreementsByLandlordIdAsync(int landlordId);
        Task<int> GetTotalAgreementCountAsync();
        Task<int> GetApprovedAgreementCountAsync();
        Task<List<Agreement>> GetExpiredAgreementsAsync();
        Task<List<Agreement>> GetExpiredAgreementsByRenterIdAsync(int renterId);
        Task UpdatePropertyStatusForAllExpiredAgreementsAsync();
    }
}
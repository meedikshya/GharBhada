using System.Threading.Tasks;
using GharBhada.Models;


namespace GharBhada.Repositories.SpecificRepositories.AgreementRepositories
{
    public interface IAgreementRepositories
    {
        Task<Agreement> GetAgreementByBookingIdAsync(int bookingId);

}
}

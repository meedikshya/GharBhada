using System.Collections.Generic;
using System.Threading.Tasks;
using GharBhada.Models;

namespace GharBhada.Repositories.SpecificRepositories.PaymentRepositories
{
    public interface IPaymentRepositories
    {
        Task<List<Payment>> GetCompletedPaymentsByLandlordIdAsync(int landlordId);
    }
}
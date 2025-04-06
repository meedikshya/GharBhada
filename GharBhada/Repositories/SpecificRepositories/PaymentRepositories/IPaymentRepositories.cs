using System.Collections.Generic;
using System.Threading.Tasks;
using GharBhada.Models;

namespace GharBhada.Repositories.SpecificRepositories.PaymentRepositories
{
    public interface IPaymentRepositories
    {
        Task<List<Payment>> GetCompletedPaymentsByLandlordIdAsync(int landlordId);
        Task<List<Payment>> GetPaymentsByAgreementIdAsync(int agreementId, string status);
        Task<bool> IsPaymentCompletedForPropertyAsync(int propertyId);
        Task<List<Payment>> GetCompletedPaymentsByRenterIdAsync(int renterId);
        Task<List<PaymentWithProperty>> GetCompletedPaymentsWithPropertyByRenterIdAsync(int renterId);
        Task<List<Property>> GetPropertiesWithCompletedPaymentsAsync();
        Task<int> GetCompletedPaymentCountAsync();
        Task<List<PaymentWithDetails>> GetCompletedPaymentsWithDetailsAsync();
    }
}
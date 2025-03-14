using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GharBhada.Data;
using GharBhada.Models;
using Microsoft.EntityFrameworkCore;

namespace GharBhada.Repositories.SpecificRepositories.PaymentRepositories
{
    public class PaymentRepositories : IPaymentRepositories
    {
        private readonly GharBhadaContext _context;

        public PaymentRepositories(GharBhadaContext context)
        {
            _context = context;
        }

        public async Task<List<Payment>> GetCompletedPaymentsByLandlordIdAsync(int landlordId)
        {
            return await _context.Payments
                .Join(_context.Agreements,
                      payment => payment.AgreementId,
                      agreement => agreement.AgreementId,
                      (payment, agreement) => new { payment, agreement })
                .Where(pa => pa.payment.PaymentStatus == "Completed" && pa.agreement.LandlordId == landlordId)
                .Select(pa => new Payment
                {
                    PaymentId = pa.payment.PaymentId,
                    AgreementId = pa.payment.AgreementId,
                    RenterId = pa.payment.RenterId,
                    Amount = pa.payment.Amount,
                    PaymentStatus = pa.payment.PaymentStatus,
                    PaymentDate = pa.payment.PaymentDate,
                    TransactionId = pa.payment.TransactionId,
                    ReferenceId = pa.payment.ReferenceId,
                    PaymentGateway = pa.payment.PaymentGateway
                })
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByAgreementIdAsync(int agreementId, string status)
        {
            return await _context.Payments
                .Where(p => p.AgreementId == agreementId && p.PaymentStatus == status)
                .ToListAsync();
        }

        public async Task<bool> IsPaymentCompletedForPropertyAsync(int propertyId)
        {
            return await _context.Payments
                .Join(_context.Agreements,
                      payment => payment.AgreementId,
                      agreement => agreement.AgreementId,
                      (payment, agreement) => new { payment, agreement })
                .Where(pa => pa.agreement.BookingId == propertyId && pa.payment.PaymentStatus == "Completed")
                .AnyAsync();
    }

}
}
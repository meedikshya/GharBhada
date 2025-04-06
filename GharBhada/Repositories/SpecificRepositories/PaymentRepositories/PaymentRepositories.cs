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

        public async Task<List<Payment>> GetCompletedPaymentsByRenterIdAsync(int renterId)
        {
            return await _context.Payments
                .Where(p => p.RenterId == renterId && p.PaymentStatus == "Completed")
                .ToListAsync();
        }

        public async Task<List<PaymentWithProperty>> GetCompletedPaymentsWithPropertyByRenterIdAsync(int renterId)
        {
            return await _context.Payments
                .Where(p => p.RenterId == renterId && p.PaymentStatus == "Completed")
                .Join(_context.Agreements,
                      payment => payment.AgreementId,
                      agreement => agreement.AgreementId,
                      (payment, agreement) => new { payment, agreement })
                .Join(_context.Bookings,
                      pa => pa.agreement.BookingId,
                      booking => booking.BookingId,
                      (pa, booking) => new { pa.payment, pa.agreement, booking })
                .Join(_context.Properties,
                      pab => pab.booking.PropertyId,
                      property => property.PropertyId,
                      (pab, property) => new PaymentWithProperty
                      {
                          Payment = pab.payment,
                          Property = property
                      })
                .ToListAsync();
        }

        public async Task<List<Property>> GetPropertiesWithCompletedPaymentsAsync()
        {
            return await _context.Payments
                .Where(p => p.PaymentStatus == "Completed")
                .Join(_context.Agreements,
                      payment => payment.AgreementId,
                      agreement => agreement.AgreementId,
                      (payment, agreement) => new { payment, agreement })
                .Join(_context.Bookings,
                      pa => pa.agreement.BookingId,
                      booking => booking.BookingId,
                      (pa, booking) => new { pa.payment, pa.agreement, booking })
                .Join(_context.Properties,
                      pab => pab.booking.PropertyId,
                      property => property.PropertyId,
                      (pab, property) => property)
                .ToListAsync();
        }

        public async Task<int> GetCompletedPaymentCountAsync()
        {
            return await _context.Payments
                .Where(p => p.PaymentStatus == "Completed")
                .CountAsync();
        }

        public async Task<List<PaymentWithDetails>> GetCompletedPaymentsWithDetailsAsync()
        {
            return await _context.Payments
                .Where(p => p.PaymentStatus == "Completed")
                .Join(_context.Agreements,
                      payment => payment.AgreementId,
                      agreement => agreement.AgreementId,
                      (payment, agreement) => new { payment, agreement })
                .Join(_context.Bookings,
                      pa => pa.agreement.BookingId,
                      booking => booking.BookingId,
                      (pa, booking) => new { pa.payment, pa.agreement, booking })
                .Join(_context.Properties,
                      pab => pab.booking.PropertyId,
                      property => property.PropertyId,
                      (pab, property) => new PaymentWithDetails
                      {
                          Payment = pab.payment,
                          Property = property,
                          RenterId = pab.payment.RenterId,
                          LandlordId = pab.agreement.LandlordId
                      })
                .ToListAsync();
        }
    }

    public class PaymentWithProperty
    {
        public required Payment Payment { get; set; }
        public required Property Property { get; set; }
    }

    public class PaymentWithDetails
    {
        public required Payment Payment { get; set; }
        public required Property Property { get; set; }
        public int RenterId { get; set; }
        public int LandlordId { get; set; }
    }
}
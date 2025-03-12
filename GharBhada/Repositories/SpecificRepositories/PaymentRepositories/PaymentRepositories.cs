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
            return await (from p in _context.Payments
                          join a in _context.Agreements on p.AgreementId equals a.AgreementId
                          where p.PaymentStatus == "Completed" && a.LandlordId == landlordId
                          select new Payment
                          {
                              PaymentId = p.PaymentId,
                              AgreementId = p.AgreementId,
                              RenterId = p.RenterId,
                              Amount = p.Amount,
                              PaymentStatus = p.PaymentStatus,
                              PaymentDate = p.PaymentDate,
                              TransactionId = p.TransactionId,
                              ReferenceId = p.ReferenceId,
                              PaymentGateway = p.PaymentGateway
                          }).ToListAsync();
        }
    }
}
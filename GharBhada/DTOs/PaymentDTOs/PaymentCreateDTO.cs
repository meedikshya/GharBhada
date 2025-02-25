using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.DTOs.PaymentDTOs
{
    public class PaymentCreateDTO
    {

        public int AgreementId { get; set; }

        public int RenterId { get; set; }

        public decimal Amount { get; set; }

        public required string PaymentStatus { get; set; }

    }
}

namespace GharBhada.DTOs.PaymentDTOs
{
    public class PaymentUpdateDTO
    {
        public int PaymentId { get; set; }

        public int AgreementId { get; set; }

        public int RenterId { get; set; }

        public decimal Amount { get; set; }

        public required string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? TransactionId { get; set; }
        public string? ReferenceId { get; set; }
        public string? PaymentGateway { get; set; }
    }
}

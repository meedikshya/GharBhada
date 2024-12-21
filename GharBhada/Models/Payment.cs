using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int RentalId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public required string PaymentStatus { get; set; } // "Pending", "Completed"
        public required string PaymentMethod { get; set; } // "Credit Card", "Debit Card", etc.

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
    }
}

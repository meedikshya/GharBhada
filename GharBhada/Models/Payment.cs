using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int AgreementId { get; set; }

        [ForeignKey("User")]
        public int RenterId { get; set; }

        public decimal Amount { get; set; }

        public required string PaymentStatus { get; set; } 

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime PaymentDate { get; set; } = DateTime.Now;

    }
}

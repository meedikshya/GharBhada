using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class Agreement
    {
        public int AgreementId { get; set; }

        public int BookingId { get; set; }

        [ForeignKey("User")]
        public int LandlordId { get; set; }

        [ForeignKey("User")]
        public int RenterId { get; set; }

        public required DateTime StartDate { get; set; }

        public required DateTime EndDate { get; set; }

        public  required string Status { get; set; } 

        public required DateTime? SignedAt { get; set; }

    }
}

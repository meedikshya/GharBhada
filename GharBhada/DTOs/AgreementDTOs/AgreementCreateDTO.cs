using System;
using System.ComponentModel.DataAnnotations;

namespace GharBhada.DTOs
{
    public class AgreementCreateDTO
    {
        public int BookingId { get; set; }

        public int LandlordId { get; set; }

        public int RenterId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public DateTime? SignedAt { get; set; }  
    }
}

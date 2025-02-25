namespace GharBhada.DTOs.AgreementDTOs
{
    public class AgreementUpdateDTO
    {
        public int AgreementId { get; set; }

        public int BookingId { get; set; }

        public int LandlordId { get; set; }

        public int RenterId { get; set; }

        public required DateTime StartDate { get; set; }

        public required DateTime EndDate { get; set; }

        public required string Status { get; set; }

        public required DateTime? SignedAt { get; set; }
    }
}

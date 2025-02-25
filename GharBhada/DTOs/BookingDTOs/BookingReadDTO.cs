using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.DTOs.BookingDTOs
{
    public class BookingReadDTO
    {
        public int BookingId { get; set; }

        public int UserId { get; set; }

        public int PropertyId { get; set; }

        public required string Status { get; set; }
    }
}

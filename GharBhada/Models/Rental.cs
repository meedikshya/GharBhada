using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class Rental
    {
        public int RentalId { get; set; }
        public int UserId { get; set; }
        public int PropertyId { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required decimal RentAmount { get; set; }
        public required string RentalStatus { get; set; }
        public required string BookingReference { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
    }

}

using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class Agreement
    {
        public int AgreementId { get; set; }
        public int RentalId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required string Terms { get; set; }
        public required string Status { get; set; } // "Active", "Expired"

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }

        [Required]
        [MaxLength(50)]  
        public string Status { get; set; } 

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime BookingDate { get; set; } = DateTime.Now;
    }
}

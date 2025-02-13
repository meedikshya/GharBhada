using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class MoveInAssistance
    {
        [Key]
        public int AssistanceId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)] 
        public required string ServiceType { get; set; }  

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(50)] 
        public required string Status { get; set; } 
    }
}

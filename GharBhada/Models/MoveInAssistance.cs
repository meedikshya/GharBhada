using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class MoveInAssistance
    {
        public int MoveInAssistanceId { get; set; }

        public int UserId { get; set; }

        public required string ServiceType { get; set; }  

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public required string Status { get; set; } 
    }
}

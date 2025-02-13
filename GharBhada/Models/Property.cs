using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class Property
    {
        [Key]
        public int PropertyId { get; set; }

        [ForeignKey("User")]
        public int LandlordId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        [MaxLength(255)]
        public required string District { get; set; }

        [Required]
        [MaxLength(255)]
        public required string City { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Municipality { get; set; }

        [Required]
        public int Ward { get; set; }

        [MaxLength(255)]
        public string? NearestLandmark { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(50)] 
        public required string RoomType { get; set; } 

        [Required]
        [MaxLength(50)] 
        public required string Status { get; set; } 

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int TotalBedrooms { get; set; }

        [Required]
        public int TotalLivingRooms { get; set; }

        [Required]
        public int TotalWashrooms { get; set; }

        [Required]
        public int TotalKitchens { get; set; }
    }
}

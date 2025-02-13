using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class PropertyImage
    {
        [Key]
        public int PropertyImageId { get; set; }

        [ForeignKey("Property")]
        public int PropertyId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string ImageUrl { get; set; }

    }
}
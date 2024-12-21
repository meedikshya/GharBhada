using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class Property
    {
        public int PropertyId { get; set; }
        public int UserId { get; set; }
        public required string Address { get; set; }
        public required string City { get; set; }
        public required decimal Price { get; set; }
        public string? PropertyType { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public required string Status { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
    }

}

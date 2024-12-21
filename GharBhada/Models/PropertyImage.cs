using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class PropertyImage
    {
        public int ImageId { get; set; }
        public int PropertyId { get; set; }
        public required string ImageUrl { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }
    }
}

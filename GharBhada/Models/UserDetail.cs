using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class UserDetail
    {
        [Key]
        public int UserDetailId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }
    }
}
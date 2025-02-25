using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class User
    {
        public int UserId { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public required string UserRole { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public required string FirebaseUId { get; set; }
    }
}

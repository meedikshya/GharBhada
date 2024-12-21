using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class Admin
    {

        public int AdminId { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get;set; }
        public required string LastName{ get; set; }
        public required string phone_number { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public DateTime CreatedAt{ get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class UserDetail
    {
        public int UserDetailId { get; set; }

        public int UserId { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
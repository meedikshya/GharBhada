namespace GharBhada.DTOs.UserDTOs
{
    public class UserUpdateDTO
    {
        public int UserId { get; set; }

        public required string Email { get; set; }

        public required string PasswordHash { get; set; }

        public required string UserRole { get; set; }

        public required string FirebaseUId { get; set; }

    }
}

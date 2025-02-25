namespace GharBhada.DTOs.UserDetailDTOs
{
    public class UserDetailUpdateDTO
    {
        public int UserDetailId { get; set; }
        public int UserId { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}

namespace GharBhada.DTOs.MoveInAssistanceDTOs
{
    public class MoveInAssistanceReadDTO
    {
        public int MoveInAssistanceId { get; set; }

        public int UserId { get; set; }

        public required string ServiceType { get; set; }

        public required string Status { get; set; }
    }
}

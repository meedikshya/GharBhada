using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.DTOs.MoveInAssistanceDTOs
{
    public class MoveInAssistanceCreateDTO
    {

        public int UserId { get; set; }

        public required string ServiceType { get; set; }

        public required string Status { get; set; }
    }
}

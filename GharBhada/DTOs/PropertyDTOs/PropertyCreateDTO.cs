using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.DTOs.PropertyDTOs
{
    public class PropertyCreateDTO
    {

        public int LandlordId { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }

        public required string District { get; set; }

        public required string City { get; set; }

        public required string Municipality { get; set; }

        public int Ward { get; set; }

        public string? NearestLandmark { get; set; }

        public decimal Price { get; set; }

        public required string RoomType { get; set; }

        public required string Status { get; set; }

        public int TotalBedrooms { get; set; }

        public int TotalLivingRooms { get; set; }

        public int TotalWashrooms { get; set; }

        public int TotalKitchens { get; set; }

    }
}

namespace GharBhada.DTOs.PropertyImageDTOs
{
    public class PropertyImageReadDTO
    {
        public int PropertyImageId { get; set; }
        public int PropertyId { get; set; }

        public required string ImageUrl { get; set; }
    }
}

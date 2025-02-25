namespace GharBhada.DTOs.PropertyImageDTOs
{
    public class PropertyImageUpdateDTO
    {
        public int PropertyImageId { get; set; }
        public int PropertyId { get; set; }

        public required string ImageUrl { get; set; }
    }
}

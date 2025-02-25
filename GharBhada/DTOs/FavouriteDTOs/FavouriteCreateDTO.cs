using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.DTOs.FavouriteDTOs
{
    public class FavouriteCreateDTO
    {
        public int UserId { get; set; }

        public int PropertyId { get; set; }

    }
}

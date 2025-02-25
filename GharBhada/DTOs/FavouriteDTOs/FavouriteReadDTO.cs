using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.DTOs.FavouriteDTOs
{
    public class FavouriteReadDTO
    {
        public int FavouriteId { get; set; }

        public int UserId { get; set; }

        public int PropertyId { get; set; }

    }
}

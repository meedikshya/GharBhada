using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GharBhada.Models
{
    public class PropertyImage
    {
        public int PropertyImageId { get; set; }

        public int PropertyId { get; set; }

        public required string ImageUrl { get; set; }

    }
}
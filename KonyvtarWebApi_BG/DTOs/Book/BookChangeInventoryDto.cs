using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookChangeInventoryDto
    {
        [Required]
        public int CurrentInventoryCount { get; set; }
    }
}

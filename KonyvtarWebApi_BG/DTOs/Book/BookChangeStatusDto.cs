using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookChangeStatusDto
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}

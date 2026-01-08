using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookUpdateCopiesDto
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public int NewCopies { get; set; }
    }
}

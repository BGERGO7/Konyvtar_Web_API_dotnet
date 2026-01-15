using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookUpdateCopiesDto
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public int NewCopies { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime? Modified { get; set; }
    }
}

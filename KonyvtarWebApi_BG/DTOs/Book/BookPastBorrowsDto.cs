using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookPastBorrowsDto
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public List<int> PastBorrowStudentIds { get; set; } = null!;

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime? Modified { get; set; }
    }
}

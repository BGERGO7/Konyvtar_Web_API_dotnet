using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class ManageAuthor
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }
}

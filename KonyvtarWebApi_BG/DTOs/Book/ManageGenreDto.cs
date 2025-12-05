using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class ManageGenreDto
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int GenreId { get; set; }
    }
}

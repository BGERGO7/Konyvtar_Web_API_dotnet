using System.ComponentModel.DataAnnotations.Schema;

namespace KonyvtarWebApi_BG.Models
{
    public class BookGenre
    {
        public int BookGenreId { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; } = null!;
        public int GenreId { get; set; }
        [ForeignKey("GenreId")]
        public Genre Genre { get; set; } = null!;
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}

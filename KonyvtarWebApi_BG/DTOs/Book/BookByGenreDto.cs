using KonyvtarWebApi_BG.DTOs.Stats;

namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookByGenreDto
    {
        public GenreForStatDto Genre { get; set; } = null!;
        public List<BookStatItemDto> Books { get; set; } = null!;


        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}

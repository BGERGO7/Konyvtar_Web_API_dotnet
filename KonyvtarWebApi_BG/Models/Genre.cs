namespace KonyvtarWebApi_BG.Models
{
    public class Genre
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; } = null!;
        public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
        public List<Book> Books { get; set; } = new List<Book>();
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookByGenreDto
    {
        public int BookId { get; set; }
        public string HungarianTitle { get; set; } = null!;
        public string OriginalTitle { get; set; } = null!;
        public List<int> AuthorId { get; set; } = new List<int>();
        public List<string> AuthorName { get; set; } = new List<string>();
        public List<string> Genres { get; set; } = new List<string>();
        public int CurrentlyAvailable { get; set; }
        public int ReleaseYear { get; set; }
    }
}

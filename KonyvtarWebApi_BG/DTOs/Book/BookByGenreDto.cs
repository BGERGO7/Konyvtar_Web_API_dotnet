namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookByGenreDto
    {
        public int BookId { get; set; }
        public string HungarianTitle { get; set; }
        public string OriginalTitle { get; set; }
        public List<int> AuthorId { get; set; }
        public List<string> AuthorName { get; set; }
        public List<string> Genres { get; set; }
        public int CurrentlyAvailable { get; set; }
        public int ReleaseYear { get; set; }
    }
}

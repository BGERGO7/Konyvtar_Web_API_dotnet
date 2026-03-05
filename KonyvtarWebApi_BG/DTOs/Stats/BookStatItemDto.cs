namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class BookStatItemDto
    {
        public int Id { get; set; }
        public string HungarianTitle { get; set; } = null!;
        public string OriginalTitle { get; set; } = null!;
        public List<AuthorForStatDto> Authors { get; set; } = new List<AuthorForStatDto>();
        public List<GenreForStatDto> Genres { get; set; } = new List<GenreForStatDto>();
        public int TotalCopies { get; set; }
        public int PublicationYear { get; set; }
    }
}

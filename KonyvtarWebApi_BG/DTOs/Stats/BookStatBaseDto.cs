namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class BookStatBaseDto
    {
        public int BookId { get; set; }
        public string HungarianTitle { get; set; } = null!;
        public string OriginalTitle { get; set; } = null!;
    }
}

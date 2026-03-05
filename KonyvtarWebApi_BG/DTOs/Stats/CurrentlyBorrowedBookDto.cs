// KonyvtarWebApi_BG\DTOs\Stats\CurrentlyBorrowedBookDto.cs
namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class CurrentlyBorrowedBookDto
    {
        public int BookId { get; set; }
        public string HungarianTitle { get; set; } = string.Empty;
        public string OriginalTitle { get; set; } = string.Empty;
        public StudentForStatDto Student = null!;

        public List<AuthorForStatDto> Authors = new List<AuthorForStatDto>();
        
        public List<GenreForStatDto> Genres = new List<GenreForStatDto>();
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }

        // Ez a mező fogja jelezni, hogy lejárt-e
        public bool IsOverdue { get; set; }
    }
}
// KonyvtarWebApi_BG\DTOs\Stats\CurrentlyBorrowedBookDto.cs
namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class CurrentlyBorrowedBookDto
    {
        public int BorrowId { get; set; }
        public int BookId { get; set; }
        public string HungarianTitle { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }

        // Ez a mező fogja jelezni, hogy lejárt-e
        public bool IsOverdue { get; set; }
    }
}
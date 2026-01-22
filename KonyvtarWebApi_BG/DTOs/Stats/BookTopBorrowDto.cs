namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class BookTopBorrowDto
    {
        public int BookId { get; set; }
        public string HungarianTitle { get; set; } = null!;
        public string OriginalTitle { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public int PublishedYear { get; set; }

        // The count of borrows
        public int TotalBorrows { get; set; }

        public bool Active { get; set; }
    }
}
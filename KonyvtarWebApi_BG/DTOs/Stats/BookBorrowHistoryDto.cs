namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class BookBorrowHistoryDto
    {
        public int BorrowId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        // Diák adatai
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string StudentClass { get; set; } = null!;
    }
}
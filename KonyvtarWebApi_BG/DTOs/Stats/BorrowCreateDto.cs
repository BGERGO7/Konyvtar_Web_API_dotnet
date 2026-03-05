namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class BorrowCreateDto
    {
        public int StudentId { get; set; }
        public int BookId { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? DueDate { get; set; }
    }
}

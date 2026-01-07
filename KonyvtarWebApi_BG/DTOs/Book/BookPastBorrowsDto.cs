namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookPastBorrowsDto
    {
        public int BookId { get; set; }
        public List<int>? PastBorrowStudentIds { get; set; }
    }
}

using KonyvtarWebApi_BG.DTOs.Stats;

namespace KonyvtarWebApi_BG.DTOs.Borrow
{
    public class BorrowReadDto
    {
        public int BorrowId { get; set; }
        public StudentForStatDto Student { get; set; } = null!;
        public BookStatBaseDto Book { get; set; } = null!;
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}

using KonyvtarWebApi_BG.Models;

namespace KonyvtarWebApi_BG.DTOs.Student
{
    public class StudentCurrentBorrowsDto
    {
        public int StudentId { get; set; }
        
        public List<Borrow>? Borrows { get; set; }

    }
}

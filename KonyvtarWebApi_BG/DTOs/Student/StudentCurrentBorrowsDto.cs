using KonyvtarWebApi_BG.Models;

namespace KonyvtarWebApi_BG.DTOs.Student
{
    public class StudentCurrentBorrowsDto
    {
        public int StudentId { get; set; }

        public List<StudentGetBorrows> Borrows { get; set; } = new List<StudentGetBorrows>();

        public bool Active { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}

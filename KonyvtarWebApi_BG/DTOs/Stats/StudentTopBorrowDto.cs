namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class StudentTopBorrowDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string Class { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public int TotalBorrows { get; set; }

        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}

namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class StudentTopBorrowDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Class { get; set; }
        public string EmailAddress { get; set; }
        public int TotalBorrows { get; set; }
    }
}

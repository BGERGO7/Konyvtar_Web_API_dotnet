
namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class StudentBorrowsStatsDto
    {
        // Diák azonosító adatok
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;

        // Statisztikai összegzés
        public int TotalBorrows { get; set; }
        public int ActiveBorrowsCount { get; set; }

        // A kölcsönzések listája (beágyazva)
        public List<StudentBorrowDetailsDto> Borrows { get; set; } = new List<StudentBorrowDetailsDto>();
    }
}
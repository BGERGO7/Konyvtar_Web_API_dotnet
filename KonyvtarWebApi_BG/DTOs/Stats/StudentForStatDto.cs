namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class StudentForStatDto
    {
        public int StudentId { get; set; }
        public string Name { get; set; } = null!;
        public string Class { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;

    }
}

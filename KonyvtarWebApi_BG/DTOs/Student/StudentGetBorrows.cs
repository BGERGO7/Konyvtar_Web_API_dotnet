namespace KonyvtarWebApi_BG.DTOs.Student
{
    public class StudentGetBorrows
    {
        public int StudentId { get; set; }
        public List<string> BookNames { get; set; } = new List<string>();
        public bool BookActive { get; set; }
        public DateTime BookCreated { get; set; }
        public DateTime? BookModified { get; set; }

        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}

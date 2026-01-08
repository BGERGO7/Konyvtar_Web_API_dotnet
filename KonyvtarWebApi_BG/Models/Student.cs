namespace KonyvtarWebApi_BG.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = null!;
        public string PlaceOfBirth { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; } = null!;
        public string Class { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;

        public LibraryCard LibraryCard { get; set; } = null!;
        public List<Borrow> Borrows { get; set; } = new List<Borrow>();

        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}


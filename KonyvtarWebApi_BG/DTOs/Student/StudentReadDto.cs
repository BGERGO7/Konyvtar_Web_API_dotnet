namespace KonyvtarWebApi_BG.DTOs.Student
{
    public class StudentReadDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Class { get; set; } 
        public string EmailAddress { get; set; }

        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public int? LibraryCardId { get; set; } 
        public DateTime? CardIssueDate { get; set; }
        public DateTime? CardExpirationDate { get; set; }
    }
}


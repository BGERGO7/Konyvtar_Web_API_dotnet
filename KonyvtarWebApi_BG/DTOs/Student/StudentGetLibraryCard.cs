namespace KonyvtarWebApi_BG.DTOs.Student
{
    public class StudentGetLibraryCard
    {
        public int StudentId { get; set; }
        public int LibraryCardId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool LibrayCardActive { get; set; }
        public DateTime LibraryCardCreated { get; set; }
        public DateTime? LibraryCardModified { get; set; }
    }
}

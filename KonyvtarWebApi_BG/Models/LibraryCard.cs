using System.ComponentModel.DataAnnotations.Schema;

namespace KonyvtarWebApi_BG.Models
{
    public class LibraryCard
    {
        public int LibraryCardId { get; set; }
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
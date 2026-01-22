using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.LibraryCard
{
    public class LibraryCardUpdateDto
    {
        [Required]
        public int LibraryCardId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
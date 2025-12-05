using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KonyvtarWebApi_BG.DTOs.LibraryCard
{
    public class LibraryCardCreateDto
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }
    }
}

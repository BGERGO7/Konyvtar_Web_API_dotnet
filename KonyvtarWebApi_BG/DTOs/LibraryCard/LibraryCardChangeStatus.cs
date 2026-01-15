using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.LibraryCard
{
    public class LibraryCardChangeStatus
    {
        [Required]
        public int LibraryCardId { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime? Modified { get; set; }
    }
}

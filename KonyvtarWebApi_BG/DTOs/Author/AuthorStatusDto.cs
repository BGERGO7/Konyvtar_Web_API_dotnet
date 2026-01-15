using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Author
{
    public class AuthorStatusDto
    {
        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime? Modified { get; set; }
    }
}

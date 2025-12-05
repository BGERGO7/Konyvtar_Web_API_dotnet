using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Author
{
    public class AuthorStatusDto
    {
        [Required]
        public int AuthorId { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}

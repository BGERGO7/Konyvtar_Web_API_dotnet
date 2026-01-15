using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Genre
{
    public class GenreCreateDto
    {        
        [Required]
        [MaxLength(50)]
        public string? GenreName { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime? Modified { get; set; }
    }
}
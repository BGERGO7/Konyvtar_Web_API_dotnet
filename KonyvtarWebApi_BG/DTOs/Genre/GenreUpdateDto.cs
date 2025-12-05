using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Genre
{
    public class GenreUpdateDto
    {
        [Required]
        public int GenreId { get; set; }

        [Required]
        [MaxLength(50)]
        public string GenreName { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
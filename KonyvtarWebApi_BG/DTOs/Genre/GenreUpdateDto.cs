using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Genre
{
    public class GenreUpdateDto
    {        
        [Required]
        [MaxLength(50)]
        public string GenreName { get; set; }
    }
}
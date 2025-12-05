using KonyvtarWebApi_BG.Models;
using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Author
{
    public class AuthorUpdateDto
    {
        [Required]
        public int AuthorId { get; set; }

        [Required]
        [MaxLength(100)]
        public string AuthorName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(100)]
        public string PlaceOfBirth { get; set; }

        [MaxLength(500)]
        public string Biography { get; set; }

        [Required]
        public bool Active { get; set; }

    }
}

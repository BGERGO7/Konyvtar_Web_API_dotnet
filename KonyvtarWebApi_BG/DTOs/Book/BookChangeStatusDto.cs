using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookStatusDto
    {
        [Required]
        public bool Active { get; set; }
    }
}

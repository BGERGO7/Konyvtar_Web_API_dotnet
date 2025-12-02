using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KonyvtarWebApi_BG.Models
{
    public class Genre
    {
        [Required]
        public int GenreId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string GenreName { get; set; }

        public List<Book> Books { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KonyvtarWebApi_BG.Models
{
    public class Author
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

        public List<Book> Books { get; set; }
    }
}
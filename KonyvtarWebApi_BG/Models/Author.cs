using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace KonyvtarWebApi_BG.Models
{
    public class Author
    {
        [Required]
        public int AuthorId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string? AuthorName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(100)]
        public string? PlaceOfBirth { get; set; }

        [JsonIgnore]
        public List<BookAuthor>? BookAuthors { get; set; }

        [MaxLength(500)]
        public string? Biography { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}
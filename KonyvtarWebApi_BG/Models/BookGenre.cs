using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KonyvtarWebApi_BG.Models
{
    public class BookGenre
    {
        public int BookGenreId { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        [JsonIgnore]
        public Book? Book { get; set; }
        public int GenreId { get; set; }
        [ForeignKey("GenreId")]
        [JsonIgnore]
        public Genre? Genre { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}

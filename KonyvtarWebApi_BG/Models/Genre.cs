using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KonyvtarWebApi_BG.Models
{
    public class Genre
    {
        [Required]
        public int GenreId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string GenreName { get; set; }

        [JsonIgnore]
        public List<BookGenre> BookGenres { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}
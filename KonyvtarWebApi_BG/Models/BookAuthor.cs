using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KonyvtarWebApi_BG.Models
{
    public class BookAuthor
    {
        public int BookAuthorId { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        [JsonIgnore]
        public Book? Book { get; set; }
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        [JsonIgnore]
        public Author? Author { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}

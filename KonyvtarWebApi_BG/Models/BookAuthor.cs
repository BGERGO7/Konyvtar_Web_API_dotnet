using System.ComponentModel.DataAnnotations.Schema;

namespace KonyvtarWebApi_BG.Models
{
    public class BookAuthor
    {
        public int BookAuthorId { get; set; }
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; } = null!;
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; } = null!;
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}

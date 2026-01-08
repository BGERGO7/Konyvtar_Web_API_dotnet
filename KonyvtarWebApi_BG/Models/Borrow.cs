using System.ComponentModel.DataAnnotations.Schema;

namespace KonyvtarWebApi_BG.Models
{
    public class Borrow
    {
        public int BorrowId { get; set; }
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public Student Student { get; set; } = null!;

        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; } = null!;

        public DateTime BorrowDate { get; set; }

        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    
         
    }
}
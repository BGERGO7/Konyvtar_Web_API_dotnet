using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace KonyvtarWebApi_BG.Models
{
    public class Borrow
    {
        [Required]
        public int BorrowId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        [JsonIgnore]
        public Student? Student { get; set; }

        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        [JsonIgnore]
        public Book Book { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        [Required]
        public DateTime DueDate { get; set; } //Kolcsenzes datum + Book.MaxRentDays

        public DateTime? ReturnDate { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}
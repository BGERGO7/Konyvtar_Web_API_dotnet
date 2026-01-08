using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace KonyvtarWebApi_BG.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
     
        public string? AuthorName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? PlaceOfBirth { get; set; }

        public List<BookAuthor>? BookAuthors { get; set; }
         
        public string? Biography { get; set; }

        public bool Active { get; set; }

        public List<Book> Books { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}
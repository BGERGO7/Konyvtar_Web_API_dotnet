namespace KonyvtarWebApi_BG.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
     
        public string AuthorName { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string PlaceOfBirth { get; set; } = null!;

        public List<BookAuthor> BookAuthors { get; set; } = null!;
         
        public string Biography { get; set; } = null!;

        public bool Active { get; set; }

        public List<Book> Books { get; set; } = null!;

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

    }
}
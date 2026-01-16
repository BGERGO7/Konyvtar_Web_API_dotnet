namespace KonyvtarWebApi_BG.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string HungarianTitle { get; set; } = null!;
        public string OriginalTitle { get; set; } = null!;
        public string RecommendationText { get; set; } = null!;
        public int PublishedYear { get; set; }
        public int MaxInventoryCount { get; set; }


        //try-catch a controllerben!!!
        /*
        private int _currentInventoryCount;

        public int CurrentInventoryCount {

            get => _currentInventoryCount;
            
            set
            {
                if (value < 0)
                    throw new ArgumentException("CurrentInventoryCount cannot be negative");
                if (value >= MaxInventoryCount)
                    throw new ArgumentException("CurrentInventoryCount cannot exceed MaxInventoryCount");

                _currentInventoryCount = value;
            }

        }
        */

        public int CurrentInventoryCount { get; set; }
        public string Publisher { get; set; } = null!;
        public int MaxRentDays { get; set; }
        public List<Borrow> Borrows { get; set; } = new List<Borrow>();
        public List<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
        public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public List<Author> Authors { get; set; } = new List<Author>();
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
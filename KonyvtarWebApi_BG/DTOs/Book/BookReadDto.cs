namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookReadDto
    {
        public int BookId { get; set; }
        public string HungarianTitle { get; set; } = null!;
        public string OriginalTitle { get; set; } = null!;
        public string RecommendationText { get; set; } = null!;
        public int PublishedYear { get; set; }
        public int MaxInventoryCount { get; set; }
        public int CurrentInventoryCount { get; set; }
        public string Publisher { get; set; } = null!;
        public int MaxRentDays { get; set; }

        public List<string> Authors { get; set; } = new List<string>();
        public List<string> Genres { get; set; } = new List<string>)();

        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

    }
}
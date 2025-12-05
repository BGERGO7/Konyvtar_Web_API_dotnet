namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookReadDto
    {
        public int BookId { get; set; }
        public string HungarianTitle { get; set; }
        public string OriginalTitle { get; set; }
        public string RecommendationText { get; set; }
        public int PublishedYear { get; set; }
        public int MaxInventoryCount { get; set; }
        public int CurrentInventoryCount { get; set; }
        public string Publisher { get; set; }
        public int MaxRentDays { get; set; }

        public List<string>? Authors { get; set; }
        public List<string>? Genres { get; set; }  

        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

    }
}
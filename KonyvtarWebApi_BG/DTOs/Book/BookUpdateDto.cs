using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookUpdateDto
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        [MaxLength(200)]
        public string HungarianTitle { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string OriginalTitle { get; set; } = null!;
        
        [Required]
        [MaxLength(200), MinLength(30)]
        public string RecommendationText { get; set; } = null!;
        
        [Required]
        public int PublishedYear { get; set; }
        
        [Required]
        [Range(0, int.MaxValue)]
        public int MaxInventoryCount { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CurrentInventoryCount { get; set; }
        [Required]
        public string Publisher { get; set; } = null!;

        [Required]
        public int MaxRentDays { get; set; }

        [Required]
        public List<int> AuthorIds { get; set; } = new List<int>();

        [Required]
        public List<int> GenreIds { get; set; } = new List<int>)();

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime? Modified { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Book
{
    public class BookUpdateDto
    {
        [Required]
        public int BookId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string HungarianTitle { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string OriginalTitle { get; set; }
        
        [Required]
        [MaxLength(200), MinLength(30)]
        public string RecommendationText { get; set; }
        
        [Required]
        public int PublishedYear { get; set; }
        
        [Required]
        [Range(0, int.MaxValue)]
        public int MaxInventoryCount { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int CurrentInventoryCount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CurrentInventoryCount > MaxInventoryCount)
            {
                yield return new ValidationResult(
                    "Current inventory count cannot exceed maximum inventory count.",
                    new[] { nameof(CurrentInventoryCount), nameof(MaxInventoryCount) });
            }
        }

        [Required]
        public string Publisher { get; set; }

        [Required]
        public int MaxRentDays { get; set; }

        [Required]
        public List<int>? AuthorIds { get; set; }

        [Required]
        public List<int>? GenreIds { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
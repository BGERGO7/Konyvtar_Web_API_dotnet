using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KonyvtarWebApi_BG.Models
{
    public class Book
    {
        [Required]
        public int BookId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string? HungarianTitle { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string? OriginalTitle { get; set; }
        
        [Required]
        [MaxLength(200), MinLength(30)]
        public string? RecommendationText { get; set; }
        
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
        public string? Publisher { get; set; }

        [Required]
        public int MaxRentDays { get; set; }

        [JsonIgnore]
        public List<Borrow>? Borrows { get; set; }

        [JsonIgnore]
        public List<BookAuthor>? BookAuthors { get; set; }

        [JsonIgnore]
        public List<BookGenre>? BookGenres { get; set; }


        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}
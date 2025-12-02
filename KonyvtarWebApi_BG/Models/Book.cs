using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KonyvtarWebApi_BG.Models
{
    public class Book
    {
        [Required]
        public int BookId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string HungarianTitle { get; set; }
        
        [Required]
        public string OriginalTitle { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string RecommendationText { get; set; }
        
        [Required]
        [MaxLength(200)]
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
        public List<Genre> Genres { get; set; }

        public List<Author> Authors { get; set; }

        [Required]
        public int MaxRentDays { get; set; }
    }
}
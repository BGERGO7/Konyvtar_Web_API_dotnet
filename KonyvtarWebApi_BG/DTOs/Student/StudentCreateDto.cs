using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KonyvtarWebApi_BG.DTOs.Student
{
    public class StudentCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string? StudentName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string? PlaceOfBirth { get; set; }
        
        [Required]
        public DateTime DateOfBirth { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string? Address { get; set; }
        
        [Required]
        [MaxLength(5)]
        public string? Class { get; set; }
        
        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }

    }
}


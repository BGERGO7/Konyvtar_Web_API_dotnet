using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KonyvtarWebApi_BG.Models
{
    public class Student
    {
        [Required]
        public int StudentId { get; set; }

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

        [JsonIgnore]
        public LibraryCard? LibraryCard { get; set; }

        [JsonIgnore]
        public List<Borrow>? Borrows { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }
    }
}


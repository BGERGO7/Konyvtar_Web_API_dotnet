using KonyvtarWebApi_BG.Models;
using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Author
{
    public class AuthorReadDto
    {
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? Biography { get; set; }

        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}

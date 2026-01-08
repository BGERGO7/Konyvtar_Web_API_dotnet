using KonyvtarWebApi_BG.Models;
using System.ComponentModel.DataAnnotations;

namespace KonyvtarWebApi_BG.DTOs.Author
{
    public class AuthorReadDto
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public List<int> BookIds { get; set; } = null!;

        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}

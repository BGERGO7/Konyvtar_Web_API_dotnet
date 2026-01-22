using System;
using System.Collections.Generic;

namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class AuthorGetBooksDto
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;
        public List<BookWithInventoryDto> Books { get; set; } = new();

        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}

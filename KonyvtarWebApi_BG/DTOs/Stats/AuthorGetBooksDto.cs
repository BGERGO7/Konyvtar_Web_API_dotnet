using System;
using System.Collections.Generic;

namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class AuthorGetBooksDto
    {
        public AuthorForStatDto Author { get; set; } = null!;
        public List<BookStatItemDto> Books { get; set; } = new List<BookStatItemDto>();
            
    }
}

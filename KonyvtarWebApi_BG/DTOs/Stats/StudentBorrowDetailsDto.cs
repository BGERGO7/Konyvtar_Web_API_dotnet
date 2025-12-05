using System;
using System.Collections.Generic;

namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class StudentBorrowDetailsDto
    {
        // Kölcsönzési adatok
        public int BorrowId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        
        // Könyv adatok (kilapítva)
        public int BookId { get; set; }
        public string? HungarianTitle { get; set; }
        public string? OriginalTitle { get; set; }
        
        // Szerző adatok (N:M kapcsolat kilapítva)
        public List<string>? AuthorNames { get; set; }
    }
}
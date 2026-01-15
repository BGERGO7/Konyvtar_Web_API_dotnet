namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class AuthorGetBooksDto
    {
        public int AuthorId { get; set; }
        public List<BookWithInventoryDto> Books { get; set; } = new List<BookWithInventoryDto>();

        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}

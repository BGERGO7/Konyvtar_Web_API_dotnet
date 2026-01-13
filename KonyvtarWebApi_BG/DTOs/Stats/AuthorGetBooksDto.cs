namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class AuthorGetBooksDto
    {
        public int AuthorId { get; set; }
        public List<BookWithInventoryDto> Books { get; set; } = new List<BookWithInventoryDto>();
    }
}

namespace KonyvtarWebApi_BG.DTOs.Genre
{
    public class GenreReadDto
    {
        public int GenreId { get; set; }
        public string? GenreName { get; set; }

        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
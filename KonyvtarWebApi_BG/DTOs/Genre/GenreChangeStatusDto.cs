namespace KonyvtarWebApi_BG.DTOs.Genre
{
    public class GenreChangeStatusDto
    {
        [Required]
        public int GenreId { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}

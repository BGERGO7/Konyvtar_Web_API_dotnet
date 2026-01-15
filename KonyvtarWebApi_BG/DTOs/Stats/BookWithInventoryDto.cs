namespace KonyvtarWebApi_BG.DTOs.Stats
{
    public class BookWithInventoryDto
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public int CurrentInventory { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}

namespace TreasureHuntApi.Models
{
    public class TreasureHuntResult
    {
        public int Id { get; set; }
        public int InputId { get; set; }
        public double MinFuel { get; set; }
        public string? Path { get; set; } // Optional: store the path as a string
    }
} 
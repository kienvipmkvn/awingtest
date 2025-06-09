namespace TreasureHuntApi.Models
{
    public class TreasureHuntInput
    {
        public int Id { get; set; }
        public int N { get; set; } // Rows
        public int M { get; set; } // Columns
        public int P { get; set; } // Number of chests (keys)
        public int[][] Matrix { get; set; } = default!; // Matrix of chests
    }
} 
namespace tinderr.Models
{
    public class HistoryGame:EntityBase
    {
        public string wave { get; set; }
        public int item1 { get; set; } // quy định 1 là xuân 2 là hạ
        public int item2 { get; set; } // quy định 3 là thu 4 là đông
        
    }
}

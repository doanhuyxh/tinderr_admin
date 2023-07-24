namespace tinderr.Models
{
    public class HistoryRecharge : EntityBase
    {
        public string userId { get; set; }
        public int Amount { get; set; }
        public string Des { get; set; }
        public int Type { get; set; }
    }
}

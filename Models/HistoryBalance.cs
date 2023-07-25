namespace tinderr.Models
{
    public class HistoryBalance
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Amount { get; set; }
        public DateTime CreateDate { get; set; }
        public Boolean IsRecharge { get; set; } // true là nạp false là rút
    }
}

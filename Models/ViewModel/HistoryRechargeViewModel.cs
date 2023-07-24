namespace tinderr.Models.ViewModel
{
    public class HistoryRechargeViewModel : EntityBase
    {
        public string userId { get; set; }
        public int Amount { get; set; }
        public string? Des { get; set; }
        public int Type { get; set; }
        public string? UserName { get; set; }
        public string? UserAccount { get; set; }

        public static implicit operator HistoryRechargeViewModel(HistoryRecharge historyRecharge)
        {
            return new HistoryRechargeViewModel
            {
                Id = historyRecharge.Id,
                userId = historyRecharge.userId,
                Amount = historyRecharge.Amount,
                Des = historyRecharge.Des,
                Type = historyRecharge.Type,
                CreatedDate = historyRecharge.CreatedDate,
                IsDeleted = historyRecharge.IsDeleted,
            };
        }

        public static implicit operator HistoryRecharge(HistoryRechargeViewModel vm)
        {
            return new HistoryRecharge
            {
                Id = vm.Id,
                userId = vm.userId,
                Amount = vm.Amount,
                Des = vm.Des ?? "",
                Type = vm.Type,
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
            };
        }
    }
}

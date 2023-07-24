using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace tinderr.Models.AccountViewModel
{
    public class UpdateProFileUser
    {
        public string ApplicationUserId { get; set; }

        [Display(Name = "Tên người dùng")]
        public string? Name { get; set; }

        [Display(Name = "Ngân hàng")]
        public string? BankName { get; set; }

        [Display(Name = "Số tài khoản")]
        public string? BankNumber { get; set; }
    }
}

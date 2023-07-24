using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace tinderr.Models.AccountViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Nhớ tôi?")]
        public bool RememberMe { get; set; }
    }
}

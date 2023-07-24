using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace tinderr.Models.AccountViewModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Mật khẩu mới")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("Password", ErrorMessage = "Mật khẩu mới và mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }

    }
}

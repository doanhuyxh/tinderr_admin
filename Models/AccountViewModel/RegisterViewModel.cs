using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace tinderr.Models.AccountViewModel
{
    public class RegisterViewModel
    {
        [Display(Name = "Tài khoản")]
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        public string UserName { get; set; }
        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("PasswordHash", ErrorMessage = "Mật khẩu không khớp")]
        public string? ConfirmPassword { get; set; }
        [Display(Name = "Mã mời")]
        public string InviteCode { get; set; }
        [Display(Name = "Ip")]
        public string? Ip { get; set; }
        [Display(Name = "RoleName")]
        public string? Role { get; set; }

        public static implicit operator ApplicationUser(RegisterViewModel vm)
        {
            return new ApplicationUser
            {
                UserName = vm.UserName,
                IsActive = true,
            };
        }
    }

    public class RegisterMBViewModel
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; }
        public string InviteCode { get; set; }
        public string? Ip { get; set; }

        public static implicit operator ApplicationUser(RegisterMBViewModel vm)
        {
            return new ApplicationUser
            {
                UserName = vm.UserName,
                IsActive = true,
            };
        }
    }
}

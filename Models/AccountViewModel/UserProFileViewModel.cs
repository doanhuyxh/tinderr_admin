using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace tinderr.Models.AccountViewModel
{
    public class UserProFileViewModel
    {
        public string ApplicationUserId { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public IFormFile? AvatarFile { get; set; }
        public string? AvatarPath { get; set; }
        [Display(Name = "Nhóm người dùng")]
        public string? Role { set; get; }
        [Display(Name = "Tài khoản")]
        public string? UserName { set; get; }
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string? Password { set; get; }
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Không được để trống")]
        public string? Name { set; get; }
        public bool IsActive { get; set; } = true;
        [Display(Name = "IP")]
        public string? Ip { get; set; }
        [Display(Name = "Mã mời")]
        public string? InviteCode { get; set; }
        [Display(Name = "Số lượng mời")]
        public int InvitedCount { get; set; }
        [Display(Name = "Số dư")]
        public int Balance { get; set; }
        [Display(Name = "Số lượt xem còn lại")]
        public int CountWatch { get; set; }
        [Display(Name = "Ngày tạo")]
        public DateTime CreateDate { get; set; }
        public string? Bankname { get; set; }
        public string? Banknumber { get; set; }
    }
}

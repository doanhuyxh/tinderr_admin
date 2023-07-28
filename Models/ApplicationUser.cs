using Microsoft.AspNetCore.Identity;

namespace tinderr.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string AvatartPath { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? Ip { set; get; }
        public Boolean IsNap250k { set; get; }
        public string InviteCode { get; set; }
        public int InvitedCount { get; set; }
        public int Balance { get; set; }
        public int CountWatch { get; set; }
        public string Bankname { get; set; }
        public string Banknumber { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime CreateDate { get; set; }
    }
}

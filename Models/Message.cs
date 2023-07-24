using System.ComponentModel.DataAnnotations;

namespace tinderr.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string? FromUser { get; set; }
        public string? ToUser { get; set; }
        public string? Content { get; set; }
        public bool Status { get; set; } // nếu là người dùng gửi thì satus là false còn là admin gưi thì true
        public DateTime createDate { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace tinderr.Models
{
    public class MessageInRoom
    {
        [Key]
        public int Id { get; set; }
        public string? FromUser { get; set; }
        public string? Content { get; set; }
        public int RoomId { get; set; }
    }
}

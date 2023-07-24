using System.ComponentModel.DataAnnotations;

namespace tinderr.Models
{
    public class RomChat
    {
        [Key]
        public int Id { get; set; }
        public string? RoomName { get; set; }
        public string? ListUser { get; set; }
    }
}

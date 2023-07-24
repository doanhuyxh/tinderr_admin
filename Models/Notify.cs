namespace tinderr.Models
{
    public class Notify : EntityBase
    {
        public string? Content { get; set; }
        public string? Title { get; set; }
        public bool Status { get; set; }
    }
}

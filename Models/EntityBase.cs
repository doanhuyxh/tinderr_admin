namespace tinderr.Models
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}

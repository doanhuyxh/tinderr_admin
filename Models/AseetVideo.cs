namespace tinderr.Models
{
    public class AseetVideo : EntityBase
    {
        public string VideoName { get; set; }
        public int ViewCount { get; set; }
        public string ImgAvatarPath { get; set; }
        public string VideoLinkPath { get; set; }
        public bool Status { get; set; }
        public bool Outstanding { get; set; }
        public int CategoryId { get; set; }



    }
}

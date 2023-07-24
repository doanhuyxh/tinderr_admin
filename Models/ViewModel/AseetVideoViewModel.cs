using System.ComponentModel.DataAnnotations;

namespace tinderr.Models.ViewModel
{
    public class AseetVideoViewModel : EntityBase
    {
        [Display(Name ="Tên video")]
        public string VideoName { get; set; }
        [Display(Name = "Số lượt xem")]
        public int ViewCount { get; set; }
        public string? ImgAvatarPath { get; set; }
        public string? VideoLinkPath { get; set; }
        public bool Status { get; set; }
        public bool Outstanding { get; set; }
        [Display(Name = "Danh mục video")]
        public int CategoryId { get; set; }
        public string? Base64Video { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public IFormFile? AvatarFile { get; set; }
        [Display(Name = "Video")]
        public IFormFile? VideoFile { get; set; }
        public string? CategoryName { get; set; }

        public static implicit operator AseetVideoViewModel(AseetVideo aseetVideo)
        {
            return new AseetVideoViewModel
            {
                Id = aseetVideo.Id,
                VideoLinkPath = aseetVideo.VideoLinkPath,
                VideoName = aseetVideo.VideoName,
                ViewCount = aseetVideo.ViewCount,
                CategoryId = aseetVideo.CategoryId,
                CreatedDate = aseetVideo.CreatedDate,
                ImgAvatarPath = aseetVideo.ImgAvatarPath,
                IsDeleted = aseetVideo.IsDeleted,
                Outstanding = aseetVideo.Outstanding,
                Status = aseetVideo.Status,
            };
        }

        public static implicit operator AseetVideo(AseetVideoViewModel vm)
        {
            return new AseetVideo
            {
                Id = vm.Id,
                VideoLinkPath = vm.VideoLinkPath??"",
                VideoName = vm.VideoName??"",
                ViewCount = vm.ViewCount,
                CategoryId = vm.CategoryId,
                CreatedDate = vm.CreatedDate,
                ImgAvatarPath = vm.ImgAvatarPath??"",
                IsDeleted = vm.IsDeleted,
                Outstanding = vm.Outstanding,
                Status = vm.Status,
            };
        }
    }
}

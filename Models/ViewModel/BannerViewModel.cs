using System.Reflection;

namespace tinderr.Models.ViewModel
{
    public class BannerViewModel:EntityBase
    {
        public string? Name { get; set; }
        public string? Path { get; set; }
        public bool Status { get; set; }
        public IFormFile? BannerFile { get; set; }
       


        public static implicit operator BannerViewModel(Banner banner)
        {
            return new BannerViewModel
            {
                Id = banner.Id,
                CreatedDate = banner.CreatedDate,
                IsDeleted = banner.IsDeleted,
                Name = banner.Name,
                Path = banner.Path,
                Status = banner.Status,
            };
        }

        public static implicit operator Banner(BannerViewModel vm)
        {
            return new Banner
            {
                Id = vm.Id,
                CreatedDate = vm.CreatedDate,
                IsDeleted = vm.IsDeleted,
                Name = vm.Name??"",
                Path = vm.Path??"",
                Status = vm.Status,
            };
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace tinderr.Models.ViewModel
{
    public class CategoryViewModel : EntityBase
    {
        [Display(Name = "Danh mục video")]
        public string CategoryName { get; set; }
        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }
        [Display(Name = "Số lượng video")]
        public int? CountAssetVideo { get; set; }

        public static implicit operator CategoryViewModel(Category _category)
        {
            return new CategoryViewModel
            {
                CategoryName = _category.CategoryName,
                IsActive = _category.IsActive,
                CreatedDate = _category.CreatedDate,
                Id = _category.Id,
                IsDeleted = _category.IsDeleted,
            };
        }
        public static implicit operator Category(CategoryViewModel vm)
        {
            return new Category
            {
                CategoryName = vm.CategoryName ?? "",
                IsActive = vm.IsActive,
                CreatedDate = vm.CreatedDate,
                Id = vm.Id,
                IsDeleted= vm.IsDeleted,
            };
        }

    }
}

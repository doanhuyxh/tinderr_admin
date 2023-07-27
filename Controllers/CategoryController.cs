using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using tinderr.Data;
using tinderr.Models;
using tinderr.Models.ViewModel;
using tinderr.Services;

namespace tinderr.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _icommon;

        public CategoryController(ApplicationDbContext context, ICommon icommon) { 
            _context = context;
            _icommon = icommon;
        }

        //Danh mục video
        public IActionResult Index()
        {
            ViewData["Title"] = "Danh mục video";
            return View();
        }

        [HttpGet]
        public IActionResult GetDataCategory() {
            JsonResultViewModel json = new JsonResultViewModel();
            
            var ds = from c in _context.Category
                     where c.IsDeleted == false
                     select new CategoryViewModel
                     {
                         Id  = c.Id,
                         CategoryName = c.CategoryName,
                         CreatedDate = c.CreatedDate,
                         IsActive = c.IsActive,
                         IsDeleted = c.IsDeleted,
                         CountAssetVideo = _context.AseetVideo.Where(i=>i.CategoryId == c.Id).Count(),
                     };

            json.Message = string.Empty;
            json.IsSuccess = true;
            json.Data = ds.ToList();
            return Ok(json);
        }
        [HttpGet]
        public IActionResult ChangeTypeCategory(int id)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            Category category = new Category();
            category = _context.Category.FirstOrDefault(i => i.Id == id);
            category.IsActive = !category.IsActive;
            json.Message = string.Empty;
            json.IsSuccess = true;
            json.Data = category;
            _context.Update(category);
            _context.SaveChanges();
            return Ok(json);
        }

        [HttpGet]
        public IActionResult DeleteCategory(int id)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            Category category = new Category();
            category = _context.Category.FirstOrDefault(i => i.Id == id);
            category.IsDeleted = true;
            json.Message = string.Empty;
            json.IsSuccess = true;
            json.Data = category;
            _context.Update(category);
            _context.SaveChanges();
            return Ok(json);
        }

        [HttpGet]
        public IActionResult AddEditCategory(int id)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel();
            if (id != 0)
            {
                categoryViewModel = _context.Category.FirstOrDefault(i=>i.Id == id);
            }
            return View(categoryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(CategoryViewModel vm)
        {
            JsonResultViewModel jsonResultViewModel = new JsonResultViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    Category cate = new Category();
                    if(vm.Id!=0)
                    {
                        cate = _context.Category.FirstOrDefault(i=>i.Id==vm.Id);
                        _context.Entry(cate).CurrentValues.SetValues(vm);
                        await _context.SaveChangesAsync();
                        jsonResultViewModel.Message = "Success";
                        jsonResultViewModel.IsSuccess= true;
                        jsonResultViewModel.Data = cate;
                        return Ok(jsonResultViewModel);
                    }
                    else
                    {
                        cate = vm;
                        cate.CreatedDate = DateTime.Now;
                        _context.Add(cate);
                        await _context.SaveChangesAsync();
                        jsonResultViewModel.Message = "Success";
                        jsonResultViewModel.IsSuccess = true;
                        jsonResultViewModel.Data = cate;
                        return Ok(jsonResultViewModel);
                    }
                }
                jsonResultViewModel.Message = ModelState.ErrorCount.ToString();
                jsonResultViewModel.IsSuccess = false;
                jsonResultViewModel.Data = ModelState.Values.SelectMany(i => i.Errors);
                return Ok(jsonResultViewModel);
            }
            catch (Exception ex)
            {
                jsonResultViewModel.Message = ex.Message;
                jsonResultViewModel.IsSuccess = false;
                jsonResultViewModel.Data = null;
                return Ok(jsonResultViewModel);
            }
        }


        //Danh mục video
        public IActionResult Asset()
        {
            ViewData["Title"] = "Video";
            return View();
        }
        // Lấy danh sách video
        [HttpGet]
        public IActionResult GetDataVideo()
        {
            JsonResultViewModel json = new JsonResultViewModel();

            var ds = from c in _context.AseetVideo
                     where c.IsDeleted == false
                     select new AseetVideoViewModel
                     {
                         Id = c.Id,
                         VideoName = c.VideoName,
                         CreatedDate = c.CreatedDate,
                         IsDeleted = c.IsDeleted,
                         Status = c.Status,
                         Outstanding = c.Outstanding,
                         VideoLinkPath = c.VideoLinkPath,
                         ImgAvatarPath = c.ImgAvatarPath,
                         ViewCount = c.ViewCount,
                         CategoryName = _context.Category.FirstOrDefault(i=>i.Id == c.CategoryId)!.CategoryName??""
                     };

            json.Message = string.Empty;
            json.IsSuccess = true;
            json.Data = ds.ToList();
            return Ok(json);
        }

        [HttpGet]        
        public IActionResult AddEditAsset(int id)
        {
            List<ItemDropdown> cate = (from _c in _context.Category
                                       where _c.IsDeleted == false
                                       select new ItemDropdown
                                       {
                                           Id = _c.Id,
                                           Name = _c.CategoryName
                                       }).ToList();

            AseetVideoViewModel vm = new AseetVideoViewModel();
            if(id!=0)
            {
                vm = _context.AseetVideo.FirstOrDefault(i => i.Id == id);

            }
            ViewBag.category = new SelectList(cate, "Id", "Name"); ;

            return View(vm);
        }
        //Save data video
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddEditAsset(AseetVideoViewModel vm)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            try
            {
                if (ModelState.IsValid)
                {
                    AseetVideo aseetVideo = new AseetVideo();
                    if(vm.Id!=0)
                    {
                        aseetVideo = await _context.AseetVideo.FirstOrDefaultAsync(i=>i.Id == vm.Id);

                        if(vm.VideoFile!=null)
                        {
                            aseetVideo.VideoLinkPath = await _icommon.UploadVideoBase64(vm.VideoFile);
                        }
                        
                        if(vm.AvatarFile!=null)
                        {
                            aseetVideo.ImgAvatarPath = await _icommon.UploadImgAvatarFilm(vm.AvatarFile);
                        }
                        aseetVideo.VideoName = vm.VideoName;
                        aseetVideo.ViewCount = vm.ViewCount;
                        aseetVideo.Outstanding = vm.Outstanding;
                        aseetVideo.CategoryId = vm.CategoryId ?? 0;
                        _context.Update(aseetVideo);
                        await _context.SaveChangesAsync();

                        json.Message = "";
                        json.IsSuccess = true;
                        json.Data = aseetVideo;
                        return Ok(json);
                    }
                    else
                    {
                        aseetVideo = vm;
                        aseetVideo.CreatedDate = DateTime.Now;
                        aseetVideo.IsDeleted = false;
                        aseetVideo.VideoLinkPath = await _icommon.UploadVideoBase64(vm.VideoFile);
                        aseetVideo.ImgAvatarPath = await _icommon.UploadImgAvatarFilm(vm.AvatarFile);
                        await _context.AddAsync(aseetVideo);
                        await _context.SaveChangesAsync();
                        json.Message = "";
                        json.IsSuccess = true;
                        json.Data = aseetVideo;
                        return Ok(json);
                    }
                }
                else
                {
                    json.Message = ModelState.ErrorCount.ToString();
                    json.IsSuccess = false;
                    json.Data = ModelState.Values.SelectMany(i => i.Errors);
                    return Ok(json);
                }
            }
            catch(Exception ex)
            {
                json.Message = ex.Message;
                json.IsSuccess = false;
                json.Data = vm;
                return Ok(json);
            }
        }

        [HttpGet]
        public IActionResult DeleteAsset(int id)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            AseetVideo aseetVideo = new AseetVideo();
            aseetVideo = _context.AseetVideo.FirstOrDefault(i => i.Id == id);
            aseetVideo.IsDeleted = true;
            json.Message = string.Empty;
            json.IsSuccess = true;
            json.Data = aseetVideo;
            _context.Update(aseetVideo);
            _context.SaveChanges();
            return Ok(json);
        }

        [HttpGet]
        public IActionResult ChangeActiveVideo(int id)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            AseetVideo aseetVideo = new AseetVideo();
            aseetVideo = _context.AseetVideo.FirstOrDefault(i => i.Id == id);
            aseetVideo.Status = !aseetVideo.Status;
            json.Message = string.Empty;
            json.IsSuccess = true;
            json.Data = aseetVideo;
            _context.Update(aseetVideo);
            _context.SaveChanges();
            return Ok(json);
        }
        
        [HttpGet]
        public IActionResult ChangeOutstanding(int id)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            AseetVideo aseetVideo = new AseetVideo();
            aseetVideo = _context.AseetVideo.FirstOrDefault(i => i.Id == id);
            aseetVideo.Outstanding = !aseetVideo.Outstanding;
            json.Message = string.Empty;
            json.IsSuccess = true;
            json.Data = aseetVideo;
            _context.Update(aseetVideo);
            _context.SaveChanges();
            return Ok(json);
        }
    }
}

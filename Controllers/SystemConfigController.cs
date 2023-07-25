using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using tinderr.Data;
using tinderr.Models;
using tinderr.Models.ViewModel;
using tinderr.Services;

namespace tinderr.Controllers
{
    public class SystemConfigController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _icommon;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;
        public SystemConfigController(ApplicationDbContext context, ICommon common, UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _context = context;
            _icommon = common;
            _userManager = userManager;
            _config = config;
        }

        public IActionResult Index()
        {
            ViewBag.adminChat = _config["ChatCofing:AdminChat"];
            return View();
        }
        public IActionResult Banner()
        {
            return View();
        }
        public IActionResult Notify()
        {
            return View();
        }


        // lấy data banner
        [HttpGet]
        public IActionResult GetDataBanner()
        {
            JsonResultViewModel json = new JsonResultViewModel();

            var rs = from bn in _context.Banner
                     where bn.IsDeleted == false
                     select new Banner
                     {
                         Id = bn.Id,
                         CreatedDate = bn.CreatedDate,
                         IsDeleted = bn.IsDeleted,
                         Name = bn.Name,
                         Path = bn.Path,
                         Status = bn.Status,
                     };

            json.Data = rs.ToList();
            json.IsSuccess = true;
            json.Message = "";

            return Ok(json);
        }

        // chuyển trạng thái banner
        [HttpGet]
        public IActionResult ChangeStatusBaner(int id)
        {
            JsonResultViewModel json = new JsonResultViewModel();

            try
            {
                Banner b = _context.Banner.FirstOrDefault(bn => bn.Id == id);
                b.Status = !b.Status;
                _context.Update(b);
                _context.SaveChanges();
                json.IsSuccess = true;
                json.Message = "";
                json.Data = b;
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.Message;
                json.Data = null;
                return Ok(json);
            }
        }
        // xóa banner

        [HttpGet]
        public IActionResult DeleteBanner(int id)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            try
            {
                Banner b = _context.Banner.FirstOrDefault(bn => bn.Id == id);
                b.IsDeleted = !b.IsDeleted;
                _context.Update(b);
                _context.SaveChanges();
                json.IsSuccess = true;
                json.Message = "";
                json.Data = b;
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.Message;
                json.Data = null;
                return Ok(json);
            }
        }

        //Add Edit Banner
        [HttpGet]
        public IActionResult AddEditBanner(int id)
        {
            BannerViewModel banner = new BannerViewModel();

            if (id == 0)
            {
                return View(banner);
            }
            else
            {
                banner = _context.Banner.FirstOrDefault(b => b.Id == id);
                return View(banner);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEditBanner(BannerViewModel vm)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            Banner banner = new Banner();

            try
            {
                if(vm.Id == 0)
                {
                    banner = vm;
                    banner.CreatedDate = DateTime.Now;
                    banner.Path = await _icommon.UploadBannerImg(vm.BannerFile);
                    _context.Add(banner);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    banner = _context.Banner.FirstOrDefault(i=>i.Id == vm.Id);
                    if (vm.BannerFile != null)
                    {
                        vm.Path = await _icommon.UploadBannerImg(vm.BannerFile);
                    }
                    vm.CreatedDate = banner.CreatedDate;
                    _context.Entry(banner).CurrentValues.SetValues(vm);
                     await _context.SaveChangesAsync();
                }
                json.IsSuccess = true;
                json.Message = "";
                json.Data = banner;
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.Data = null;
                json.IsSuccess = false;
                return Json(json);
            }
        }

        //Lấy dữ liệu thông báo
        [HttpGet]
        public IActionResult GetDataNotify()
        {
            JsonResultViewModel json = new JsonResultViewModel();

            var ds = from no in _context.Notify
                     where no.IsDeleted == false
                     select no;

            json.Data = ds.ToList();
            json.Message = "";
            json.IsSuccess = true;
            return Ok(json);
        }

        public IActionResult AddEditNotify(int id)
        {
            Notify no = new Notify();

            if (id == 0)
            {
                return View(no);
            }
            else
            {
                no = _context.Notify.FirstOrDefault(i=>i.Id == id);
                return View(no);
            }
        }

        [HttpPost]
        public IActionResult AddEditNotify(Notify vm)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            Notify no = new Notify();
            if (vm.Id == 0)
            {
                no = vm;
                no.CreatedDate = DateTime.Now;
                _context.Add(no);
                _context.SaveChanges();
                json.Message = "";
                json.IsSuccess = true;
                json.Data = no;
                return Ok(json);
            }
            else
            {
                no = _context.Notify.FirstOrDefault(i => i.Id == vm.Id);
                vm.CreatedDate = no.CreatedDate;
                _context.Entry(no).CurrentValues.SetValues(vm);
                _context.SaveChanges();
                json.Message = "";
                json.IsSuccess = true;
                json.Data = no;
                return Ok(json);
            }
        }

        [HttpGet]
        public IActionResult ChangSatusNotify(int id)
        {
            JsonResultViewModel json = new JsonResultViewModel();

            try
            {
                Notify b = _context.Notify.FirstOrDefault(bn => bn.Id == id);
                b.Status = !b.Status;
                _context.Update(b);
                _context.SaveChanges();
                json.IsSuccess = true;
                json.Message = "";
                json.Data = b;
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.Message;
                json.Data = null;
                return Ok(json);
            }
        }

        [HttpGet]
        public IActionResult DeleteNotify(int id)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            try
            {
                Notify b = _context.Notify.FirstOrDefault(bn => bn.Id == id);
                b.IsDeleted = !b.IsDeleted;
                _context.Update(b);
                _context.SaveChanges();
                json.IsSuccess = true;
                json.Message = "";
                json.Data = b;
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.Message;
                json.Data = null;
                return Ok(json);
            }
        }

        //Config hệ thống
        [HttpGet]
        public IActionResult ListAddminChat()
        {
            JsonResultViewModel json = new();
            try
            {
                json.IsSuccess = true;
                json.Message = "Success";

                var user = from u in _userManager.GetUsersInRoleAsync("Admin").Result
                           select new
                           {
                               name = u.Name,
                               userName = u.UserName,
                           };

                json.Data = user;

                return Ok(json);
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.Message;
                return Ok(json);
            }
        }
    }
}

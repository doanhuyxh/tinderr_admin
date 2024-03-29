﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.JSInterop.Implementation;
using SharpCompress.Common;
using Swashbuckle.AspNetCore.Annotations;
using System.Xml.Linq;
using tinderr.Data;
using tinderr.Models;
using tinderr.Models.AccountViewModel;
using tinderr.Models.ViewModel;
using tinderr.Services;

namespace tinderr.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]

    public class MobileAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginViewModel> _logger;
        private readonly IConfiguration _iConfiguration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommon _icommon;
        private readonly int priceVideo;
        private readonly bool videoActivePrice;
        private readonly object FileReader;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MobileAPIController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, ILogger<LoginViewModel> logger, IConfiguration iConfiguration, ICommon common, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
            _iConfiguration = iConfiguration;
            _userManager = userManager;
            _icommon = common;
            priceVideo = int.Parse(_iConfiguration["Video:Price"] ?? "");
            videoActivePrice = bool.Parse(_iConfiguration["Video:videoActivePrice"] ?? "true");
            _webHostEnvironment = webHostEnvironment;
        }

        [SwaggerOperation("Register", Summary = "Đăng ký")]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterMBViewModel vm)
        {
            JsonResultViewModel json = new JsonResultViewModel();

            try
            {
                var checkExist = _context.ApplicationUser.FirstOrDefault(i => i.InviteCode == vm.InviteCode);

                if (checkExist == null)
                {
                    json.IsSuccess = true;
                    json.Message = "Illegal InviteCode";
                    json.Data = null;
                    return Ok(json);
                }

                //tăng lượt giới thiệu cho người mời
                checkExist.InvitedCount += 1;
                await _userManager.UpdateAsync(checkExist);

                // tạo người mới
                ApplicationUser user = vm;
                user.AvatartPath = "/upload/avatar/blank_avatar.png";
                user.Ip = vm.Ip;
                user.CreateDate = DateTime.Now;
                user.InviteCode = _icommon.RandomString(5);
                user.CountWatch = 2;
                user.IsActive = true;
                user.Balance = 0;
                user.Banknumber = "";
                user.Bankname = "";
                user.IsNap250k = false;

                var result = await _userManager.CreateAsync(user, vm.PasswordHash);

                if (result.Succeeded)
                {
                    // Set role member
                    await _userManager.AddToRoleAsync(user, "Member");

                    // trả kết quả về
                    json.IsSuccess = true;
                    json.Message = "";
                    json.Data = user;

                    return Ok(json);
                }

                foreach (var error in result.Errors)
                {
                    json.Message += error.Description;
                }
                json.IsSuccess = false;
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message = "Exception: " + ex.Message;
                json.IsSuccess = false;
                json.Data = vm;
                return Ok(json);
            }

        }

        [SwaggerOperation("Login", Summary = "Đăng nhập")]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            var user = await _userManager.FindByNameAsync(vm.UserName);

            if (user == null)
            {
                json.IsSuccess = false;
                json.Data = vm;
                json.Message = "User not found";
                return Ok(json);
            }

            if (user.IsActive == false)
            {
                json.IsSuccess = true;
                json.Data = vm;
                json.Message = "User not Active";
                return Ok(json);
            }

            json.Message = "";
            json.IsSuccess = true;

            user.LastLogin = DateTime.Now;
            await _userManager.UpdateAsync(user);
            json.Data = new
            {
                avatartPath = user.AvatartPath,
                name = user.Name,
                inviteCode = user.InviteCode,
                balance = user.Balance,
                countWatch = user.CountWatch,
                userName = user.UserName,
                bankname = user.Bankname,
                banknumber = user.Banknumber,
                id = user.Id,
            };

            return Ok(json);

        }

        [SwaggerOperation("CategoryVideo", Summary = "Danh mục film")]
        [HttpGet("categoryVideo")]
        public IActionResult GetCategory()
        {
            var rs = from cate in _context.Category
                     where cate.IsDeleted == false && cate.IsActive == true
                     select new CategoryViewModel
                     {
                         Id = cate.Id,
                         CategoryName = cate.CategoryName,
                         CountAssetVideo = _context.AseetVideo.Where(i => i.CategoryId == cate.Id).Count(),
                     };
            JsonResultViewModel json = new JsonResultViewModel();
            json.Message = "";
            json.IsSuccess = true;
            json.Data = rs.ToList();
            return Ok(json);
        }

        [SwaggerOperation("VideoHome", Summary = "Video ở trang chủ")]
        [HttpGet("videoHome")]
        public IActionResult Video()
        {
            var rs = from vi in _context.AseetVideo
                     where vi.IsDeleted == false && vi.Status == true && vi.Outstanding == true
                     select new AseetVideoViewModel
                     {
                         Id = vi.Id,
                         VideoName = vi.VideoName,
                         VideoLinkPath = vi.VideoLinkPath,
                         ViewCount = vi.ViewCount,
                         ImgAvatarPath = vi.ImgAvatarPath,
                         CreatedDate = vi.CreatedDate,
                         CategoryId = vi.CategoryId,
                     };
            JsonResultViewModel json = new JsonResultViewModel();
            json.Message = "";
            json.IsSuccess = true;
            json.Data = rs.ToList();
            return Ok(json);
        }

        [SwaggerOperation("ALlVideo", Summary = "Tất cả video")]
        [HttpGet("allVideo")]
        public IActionResult AllVideo()
        {
            var rs = from vi in _context.AseetVideo
                     where vi.IsDeleted == false && vi.Status == true
                     select new AseetVideoViewModel
                     {
                         Id = vi.Id,
                         VideoName = vi.VideoName,
                         VideoLinkPath = vi.VideoLinkPath,
                         ViewCount = vi.ViewCount,
                         ImgAvatarPath = vi.ImgAvatarPath,
                         CreatedDate = vi.CreatedDate,
                         CategoryId = vi.CategoryId,
                     };
            JsonResultViewModel json = new JsonResultViewModel();
            json.Message = "";
            json.IsSuccess = true;
            json.Data = rs.ToList();
            return Ok(json);
        }

        [SwaggerOperation("WatchVideo", Summary = "xem video")]
        [HttpGet("watchVideo")]
        public async Task<IActionResult> WatchVideo(int videoId, string userId)
        {

            JsonResultViewModel json = new JsonResultViewModel();
            json.Data = null;
            var user = await _userManager.FindByIdAsync(userId);

            if (user.CountWatch > 0)
            {
                AseetVideo asset = new AseetVideo();
                asset = _context.AseetVideo.FirstOrDefault(i => i.Id == videoId);
                asset.ViewCount += 1;
                _context.Update(asset);
                await _context.SaveChangesAsync();
                user.CountWatch -= 1;
                await  _userManager.UpdateAsync(user);
                json.Message = "Allowed";
                json.IsSuccess = true;
                return Ok(json);
            }
            else if (user.IsNap250k == true)
            {

                AseetVideo asset = new AseetVideo();
                asset = _context.AseetVideo.FirstOrDefault(i => i.Id == videoId);
                asset.ViewCount += 1;
                _context.Update(asset);
                await _context.SaveChangesAsync();
                json.Message = "Allowed";
                json.IsSuccess = true;
                return Ok(json);
            }

            json.IsSuccess = false;
            json.Message = "Not Allow";
            return Ok(json);
        }

        [SwaggerOperation("WatchVideoById", Summary = "xem video")]
        [AllowAnonymous]
        [HttpGet("id/{id}")]
        public async Task<IActionResult> VideobyId(int id)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            json.Message = "";
            json.IsSuccess = true;
            try
            {
                var data = await _context.AseetVideo.FirstOrDefaultAsync(i => i.Id == id);
                //json.Data = await System.IO.File.ReadAllTextAsync(_webHostEnvironment.ContentRootPath + "/wwwroot" + data.VideoLinkPath);
                json.Data = data.VideoLinkPath;
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

        [SwaggerOperation("Banner", Summary = "Banner")]
        [HttpGet("banner")]
        public async Task<IActionResult> GetBanner()
        {
            var json = new JsonResultViewModel();

            var data = await (from bn in _context.Banner
                              where bn.Status == true && bn.IsDeleted == false
                              select new Banner
                              {
                                  Name = bn.Name,
                                  Path = bn.Path,
                                  Status = bn.Status,
                                  CreatedDate = bn.CreatedDate,
                                  IsDeleted = bn.IsDeleted,
                                  Id = bn.Id
                              }).ToListAsync();

            json.IsSuccess = true;
            json.Message = "";
            json.Data = data;

            return Ok(json);
        }

        [SwaggerOperation("Notify", Summary = "Notify")]
        [HttpGet("notify")]
        public async Task<IActionResult> GetNotify()
        {
            var json = new JsonResultViewModel();

            var data = await (from bn in _context.Notify
                              where bn.Status == true && bn.IsDeleted == false
                              select new
                              {
                                  Title = bn.Title,
                                  Content = bn.Content
                              }).ToListAsync();

            json.IsSuccess = true;
            json.Message = "";
            json.Data = data;

            return Ok(json);
        }

        [SwaggerOperation("UpdateUser", Summary = "UpdateUser")]
        [HttpPost("updateUser")]

        public async Task<IActionResult> UpdateUser(UpdateProFileUser vm)
        {
            JsonResultViewModel json = new JsonResultViewModel();

            try
            {
                var user = await _userManager.FindByIdAsync(vm.ApplicationUserId);
                user.Name = vm.Name;
                user.Bankname = vm.BankName;
                user.Banknumber = vm.BankNumber;
                user.AvatartPath = vm.avatar;
                await _userManager.UpdateAsync(user);

                json.IsSuccess = true;
                json.Message = "";
                json.Data = user;
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.Message;
                json.Data = ex.Data;
                return Ok(json);
            }
        }

        [SwaggerOperation("UpdateBalace", Summary = "UpdateBalace")]
        [HttpGet("UpdateBalace")]
        public async Task<IActionResult> UpdateBalance(string userId)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                json.Data = user.Balance;
                json.IsSuccess = true;
                json.Message = "success";
                return Ok(json);

            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.Message;
                json.Data = ex.Data;
                return Ok(json);
            }
        }

        [SwaggerOperation("HistoryGame", Summary = "HistoryGame")]
        [HttpGet("historyGame")]
        public async Task<IActionResult> HistoryGame()
        {
            JsonResultViewModel json = new JsonResultViewModel();
            try
            {
                json.IsSuccess = true;
                json.Message = "success";

                var rs = await (from h in _context.HistoryGame
                                select new
                                {
                                    item1 = h.item1 == 1 ? "Xuân" : "Hạ",
                                    item2 = h.item2 == 3 ? "Thu" : "Đông",
                                    wave = h.wave,
                                }).ToListAsync();
                json.Data = rs.OrderByDescending(x => x.wave).TakeLast(30);
                return Ok(json);

            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.Message;
                json.Data = ex.Data;
                return Ok(json);
            }
        }

        [SwaggerOperation("ResultGame", Summary = "ResultGame")]
        [HttpPost("ResultGameUser")]
        public async Task<IActionResult> ResultGameUser(ResultGameUser req)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            try
            {
                json.IsSuccess = true;
                json.Message = "success";

                var user = await _userManager.FindByIdAsync(req.UserId);

                if (user == null)
                {
                    json.IsSuccess = false;
                    json.Message = "Not found";
                    json.Data = null;
                    return Ok(json);
                }

                if (req.status == true)
                {
                    user.Balance += req.Amount;
                    await _userManager.UpdateAsync(user);
                    json.Data = user;

                }
                else
                {
                    user.Balance -= req.Amount;
                    await _userManager.UpdateAsync(user);
                    json.Data = user;
                }

                return Ok(json);

            }
            catch (Exception ex)
            {
                json.IsSuccess = false;
                json.Message = ex.Message;
                json.Data = ex.Data;
                return Ok(json);
            }
        }
    }
}

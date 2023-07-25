using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using tinderr.Data;
using tinderr.Models;
using tinderr.Models.AccountViewModel;
using tinderr.Services;

namespace tinderr.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UserManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommon _icommon;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerController(ApplicationDbContext context, ICommon icommon, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _icommon = icommon;
            _userManager = userManager;
        }

        //quản lý người dùng role admin
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAllDataUser()
        {
            JsonResultViewModel json = new JsonResultViewModel();
            var users = _context.ApplicationUser.ToList();
            var roles = _context.Roles.ToList();
            var ds = from u in users
                     select new UserProFileViewModel
                     {
                         AvatarPath = u.AvatartPath,
                         UserName = u.UserName,
                         Name = u.Name,
                         IsActive = u.IsActive,
                         Ip = u.Ip,
                         InviteCode = u.InviteCode,
                         InvitedCount = u.InvitedCount,
                         Balance = u.Balance,
                         Bankname = u.Bankname,
                         Banknumber = u.Banknumber,
                         CountWatch = u.CountWatch,
                         ApplicationUserId = u.Id,
                         CreateDate = u.CreateDate,
                         Role = roles.FirstOrDefault(r => _userManager.IsInRoleAsync(u, r.Name).Result)?.Name // Retrieve the first role of the use
                     };
            json.IsSuccess = true;
            json.Message = "";
            json.Data = ds.ToList();
            return Ok(json);
        }

        [HttpGet]
        public IActionResult AddEditUser(string id)
        {
            UserProFileViewModel user = new UserProFileViewModel();
            if (id == "0")
            {
                user.InviteCode = _icommon.RandomString(5);
                return View(user);
            }
            else
            {
                var _user = _context.ApplicationUser.FirstOrDefault(x => x.Id == id); 
                user.Ip = _user.Ip??"";
                user.UserName = _user.Id;
                user.IsActive = true;
                user.AvatarPath = _user.AvatartPath;
                user.Name = _user.Name;
                user.ApplicationUserId = _user.Id;
                user.Balance = _user.Balance;
                user.CountWatch = _user.CountWatch;
                user.InviteCode = _user.InviteCode;
                return View(user);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AddEditUser(UserProFileViewModel vm)
        {
            JsonResultViewModel json = new JsonResultViewModel();

            ApplicationUser user = new ApplicationUser();

            bool check = await _userManager.FindByNameAsync(vm.UserName) == null;

            if (check)
            {
                user.Ip = vm.Ip;
                user.UserName = vm.UserName;
                user.Name = vm.Name;
                user.IsActive = true;
                user.AvatartPath = await _icommon.UploadAvatarUser(vm.AvatarFile);
                user.InviteCode = vm.InviteCode;
                user.InvitedCount = 0;
                user.Balance = vm.Balance;
                user.CountWatch = vm.CountWatch;
                user.Bankname = vm.Bankname;
                user.Banknumber = vm.Banknumber;
                
                await _userManager.CreateAsync(user, vm.Password);
                await _userManager.AddToRoleAsync(user, vm.Role);


                json.Message = "";
                json.IsSuccess = true;
                json.Data = user;

                return Ok(json);
            }

            user.Name = vm.Name;
            user.Balance = vm.Balance;
            user.Banknumber= vm.Banknumber;
            user.Bankname = vm.Bankname;

            if(vm.AvatarFile != null)
            {
                user.AvatartPath = await _icommon.UploadAvatarUser(vm.AvatarFile);
            }

            await _userManager.UpdateAsync(user);
            json.IsSuccess = true;
            json.Data = vm;
            json.Message = "";
            return Ok(json);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeActiveUser(string userName)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                user.IsActive = !user.IsActive;
                json.IsSuccess = true;
                json.Data = user;
                json.Message = "";
                await _userManager.UpdateAsync(user);
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message += ex.Message;
                json.Data = userName;
                json.IsSuccess = false;
                return Ok(json);
            }
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
                json.Message = "Success";
                json.Data = null;
                json.IsSuccess = true;
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message += ex.Message;
                json.Data = null;
                json.IsSuccess = false;
                return Ok(json);
            }
        }

        public async Task<IActionResult> EditUser(string id)
        {
            EditUser edit = new EditUser();
            var user = await _userManager.FindByIdAsync(id);
            edit.ApplicationUserId = user.Id;
            edit.Name = user.Name;
            edit.Bankname = user.Bankname;
            edit.Banknumber = user.Banknumber;
            edit.Balance = user.Balance;
            return View(edit);
        }

        [HttpPost]
        public async Task <IActionResult> UpdateUser(EditUser edit)
        {
            var user = await _userManager.FindByIdAsync(edit.ApplicationUserId);
            JsonResultViewModel json = new JsonResultViewModel();
            if (user != null)
            {
                user.Name = edit.Name;
                user.Bankname = edit.Bankname;
                user.Banknumber = edit.Banknumber;
                user.Balance = edit.Balance;
                if(edit.AvatarFile != null)
                {
                    user.AvatartPath = await _icommon.UploadAvatarUser(edit.AvatarFile);
                }
                await _userManager.UpdateAsync(user);

                var currenrol = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currenrol);
                await _userManager.AddToRoleAsync(user, "Admin");

                json.Message = "";
                json.Data = user;
                json.IsSuccess = true;
                return Ok(json);


            }

            json.Message = "Fail";
            json.Data = user;
            json.IsSuccess = false;
            return Ok(json);
        }

        // quản lý nhóm quyền người dùng
        public IActionResult RoleClaim()
        {
            return View();
        }
    }
}

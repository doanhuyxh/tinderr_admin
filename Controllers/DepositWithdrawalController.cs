using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using tinderr.Data;
using tinderr.Models;

namespace tinderr.Controllers
{
    public class DepositWithdrawalController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DepositWithdrawalController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //rút tiền
        public IActionResult Withdrawal()
        {
            return View();
        }

        //nạp tiền
        public IActionResult Deposit()
        {
            return View();
        }

        // lịch sử nạp tiền
        [HttpGet]
        public IActionResult HistoryDeposit()
        {
            JsonResultViewModel json = new JsonResultViewModel();

            var rs = (from h in _context.HistoryBalance
                      where h.IsRecharge == true
                      select h).ToList();

            json.IsSuccess = true;
            json.Data = rs;
            json.Message = "";
            return Ok(json);

        }

        // lịch sử rút tiền
        [HttpGet]
        public IActionResult HistoryWithdrawal()
        {
            JsonResultViewModel json = new JsonResultViewModel();

            var rs = (from h in _context.HistoryBalance
                      where h.IsRecharge == false
                      select h).ToList();

            json.IsSuccess = true;
            json.Data = rs;
            json.Message = "";
            return Ok(json);

        }

        //Nạp tiền
        [HttpGet]
        public async Task<IActionResult> DepositBalance(string userName, int amount)
        {
            JsonResultViewModel json = new JsonResultViewModel();

            var user = await _userManager.FindByNameAsync(userName);
            try
            {
                user.Balance += amount;
                await _userManager.UpdateAsync(user);

                json.IsSuccess = true;
                json.Data = user;
                json.Message = "";
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message += ex.ToString();
                json.IsSuccess = false;
                json.Data = null;
                return Ok(json);
            }
        }

        //Rút tiền
        [HttpGet]
        public async Task<IActionResult> WithdrawalBalance(string userName, int amount)
        {
            JsonResultViewModel json = new JsonResultViewModel();

            var user = await _userManager.FindByNameAsync(userName);
            try
            {
                user.Balance -= amount;
                await _userManager.UpdateAsync(user);

                json.IsSuccess = true;
                json.Data = user;
                json.Message = "";
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message += ex.ToString();
                json.IsSuccess = false;
                json.Data = null;
                return Ok(json);
            }
        }

        [HttpGet]
        public IActionResult getAllUser()
        {
            JsonResultViewModel json = new JsonResultViewModel();

            var rs = from u in _context.Users
                     select new
                     {
                         userName = u.UserName,
                         name = u.Name,
                     };
            json.Message = "";
            json.IsSuccess = true;
            json.Data = rs.ToList();

            return Ok(json);
        }
    }
}

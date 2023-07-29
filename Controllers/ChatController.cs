using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tinderr.Data;
using tinderr.Models;

namespace tinderr.Controllers
{
    public class ChatController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public ChatController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            ViewBag.currentUser = HttpContext.User.Identity.Name;
            return View();
        }


        public async Task<IActionResult> GetDataChaOneUser(string user)
        {
            JsonResultViewModel result = new JsonResultViewModel();

            var ds = await (from ms in _context.Message
                            where ms.FromUser == user || ms.ToUser == user
                            select new Message
                            {
                                FromUser = ms.FromUser,
                                ToUser = ms.ToUser,
                                createDate = ms.createDate,
                                Content = ms.Content,
                                Id = ms.Id,
                            }).ToListAsync();

            result.Data = ds;
            result.IsSuccess = true;
            result.Message = "";
            return Ok(result);
        }

        public async Task<IActionResult> historyChat(string fromUser, string toUser)
        {
            var hs = await (from ms in _context.Message
                            where (ms.FromUser == fromUser && ms.ToUser == toUser) || (ms.FromUser == toUser && ms.ToUser == fromUser)
                            select new Message
                            {
                                FromUser = ms.FromUser,
                                ToUser = ms.ToUser,
                                createDate = ms.createDate,
                                Content = ms.Content,
                                Id = ms.Id,
                                Status = ms.Status,
                            }).ToListAsync();

            return Ok(hs);
        }

        [AllowAnonymous]
        public IActionResult SaveOtherUser(string name)
        {
            //Message message = new Message();
            //message.createDate = DateTime.Now;
            //message.ToUser = name;
            //message.FromUser = _configuration["ChatCofing:AdminChat"] ?? "supperadmin";
            //message.Status = false;
            //message.Content = "Bạn là người có nhu cầu sinh lý cao ? Bạn muốn có 01 mối quan hệ không ràng buộc ? Bạn muốn kết nối với bạn tình gần khu vực bạn ? Chúng tôi có dịch vụ tại 63 tỉnh thành. Hãy để chúng tôi kết nối với các cô gái xinh đẹp tại nơi bạn sinh sống nhé !";
            //_context.Add(message);
            //_context.SaveChanges();

            //Message message1 = new Message();
            //message1.createDate = DateTime.Now;
            //message1.ToUser = name;
            //message1.FromUser = _configuration["ChatCofing:AdminChat"] ?? "supperadmin";
            //message1.Status = false;
            //message1.Content = "Em chào anh! Em là CSKH Quỳnh Anh! Hân hạnh được đón tiếp anh ạ. Anh iu đang có nhu cầu tình bạn tình, bạn nhậu đúng không ạ ?";
            //_context.Add(message1);
            //_context.SaveChanges();

            OtherUserChat us = new OtherUserChat();
            us.Name = name;
            _context.Add(us);
            _context.SaveChanges();
            return Ok(us);
        }

        public IActionResult GetOtherUser()
        {
            var ds = from o in _context.OtherUserChat
                     select new
                     {
                         name = o.Name,
                     };

            return Ok(ds.ToList());
        }

        public async Task<IActionResult> GetHistoryChatOtherUser(string name)
        {
            var hs = await (from ms in _context.Message
                            where (ms.FromUser == name || ms.ToUser == name)
                            select new Message
                            {
                                FromUser = ms.FromUser,
                                ToUser = ms.ToUser,
                                createDate = ms.createDate,
                                Content = ms.Content,
                                Id = ms.Id,
                                Status = ms.Status,
                            }).ToListAsync();

            return Ok(hs);
        }

        public IActionResult GetAvatarAndName(string userName)
        {
            JsonResultViewModel json = new JsonResultViewModel();
            try
            {
                var user = _context.Users.FirstOrDefault(i => i.UserName == userName);
                json.IsSuccess = true;
                json.Message = "";
                json.Data = new
                {
                    avatar = user.AvatartPath??"",
                    name = user.Name,
                };
                return Ok(json);
            }
            catch (Exception ex)
            {
                json.Message = ex.Message;
                json.IsSuccess= false;
                json.Data = null;
                return BadRequest(json);
            }

        }
    }
}

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
        private  readonly ApplicationDbContext _context;
        public ChatController(ApplicationDbContext context) {
            _context= context;
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
                     where (ms.FromUser == fromUser && ms.ToUser == toUser) ||(ms.FromUser == toUser && ms.ToUser== fromUser)
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
            var hs = await(from ms in _context.Message
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
    }
}

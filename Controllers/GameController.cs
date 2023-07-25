using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tinderr.Data;
using tinderr.Models;

namespace tinderr.Controllers
{
    public class GameController : Controller
    {

        private ApplicationDbContext _context;

        public GameController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> getHistory()
        {
            JsonResultViewModel json = new JsonResultViewModel();
            json.IsSuccess = true;
            json.Message = string.Empty;


            var rs = await (from h in _context.HistoryGame
                           select new
                           {
                               xuan = h.item1 ? "Xuân" : "",
                               ha = h.item2 ? "hạ" : "",
                               thu = h.item3 ? "thu" : "",
                               dong = h.item4 ? "đông" : "",
                           }).ToListAsync();
            json.Data = rs;

            return Ok(json);
        }
    }
}

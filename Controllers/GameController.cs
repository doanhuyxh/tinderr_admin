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
                                item1 = h.item1 ==1 ? "xuân":"hạ",
                                item2 = h.item2 ==3 ? "thu":"đông",
                                wave = h.wave,
                                date = h.CreatedDate
                            }).ToListAsync();
            json.Data = rs;

            return Ok(json);
        }
    }
}

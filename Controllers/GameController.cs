using Microsoft.AspNetCore.Mvc;

namespace tinderr.Controllers
{
    public class GameController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

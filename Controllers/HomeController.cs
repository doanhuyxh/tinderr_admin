using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using tinderr.Models;

namespace tinderr.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [DisableRequestSizeLimit]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadChunk(IFormFile file, long offset, string filename)
        {
            try
            {
                var webRoot = _hostingEnvironment.WebRootPath; // _hostingEnvironment là một instance của IWebHostEnvironment
                var path = Path.Combine(webRoot, "upload", "Video", filename);


                using (var stream = new FileStream(path, offset == 0 ? FileMode.Create : FileMode.Append))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(webRoot);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }

        }
    }
}
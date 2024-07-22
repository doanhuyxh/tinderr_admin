using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using tinderr.Models;

namespace tinderr.Controllers
{
    //[Authorize]
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

        [HttpGet("stream")]
        public IActionResult StreamVideo()
        {
            var webRoot = _hostingEnvironment.WebRootPath;
            var filePath = Path.Combine(webRoot, "upload", "Video", "12.mp4");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileLength = fileStream.Length;
            var rangeHeader = Request.Headers["Range"].ToString();

            if (string.IsNullOrEmpty(rangeHeader))
            {
                return new FileStreamResult(fileStream, "video/mp4")
                {
                    EnableRangeProcessing = true
                };
            }

            var range = rangeHeader.Replace("bytes=", "").Split('-');
            var start = Convert.ToInt64(range[0]);
            var end = range.Length > 1 && !string.IsNullOrEmpty(range[1]) ? Convert.ToInt64(range[1]) : fileLength - 1;

            if (start >= fileLength || end >= fileLength || start > end)
            {
                return BadRequest();
            }

            fileStream.Seek(start, SeekOrigin.Begin);
            var contentLength = end - start + 1;

            Response.StatusCode = 206; // Partial content
            Response.ContentType = "video/mp4";
            Response.Headers.Add("Content-Range", $"bytes {start}-{end}/{fileLength}");
            Response.Headers.Add("Accept-Ranges", "bytes");
            Response.ContentLength = contentLength;

            return File(fileStream, "video/mp4", enableRangeProcessing: true);
        }
    }
}


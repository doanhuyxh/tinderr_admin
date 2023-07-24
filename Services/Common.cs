using System.Text;
using System;
using tinderr.Data;

namespace tinderr.Services
{
    public class Common : ICommon
    {
        private readonly IWebHostEnvironment _iHostingEnvironment;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public Common(IWebHostEnvironment iHostingEnvironment, ApplicationDbContext context, IConfiguration configuration)
        {
            _iHostingEnvironment = iHostingEnvironment;
            _context = context;
            _configuration = configuration;
        }

        private static Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                stringBuilder.Append(chars[index]);
            }

            return stringBuilder.ToString();
        }

        public async Task<string> UploadImgAvatarFilm(IFormFile file)
        {
            string PathFile = string.Empty;
            string basePath = "/upload/ImgAvavtarVideo/";
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/ImgAvavtarVideo");

                if (file.FileName == null)
                    PathFile = "default.png";
                else
                    PathFile = DateTime.Now.Ticks.ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, PathFile);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return basePath+PathFile;

        }
        public async Task<string> UploadVideoBase64(IFormFile file)
        {
            string pathFile = string.Empty;
            string basePath = "/upload/VideoBase64/";
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/VideoBase64");
                if (file.FileName == null)
                    pathFile = "default.txt";
                else
                {
                    pathFile = DateTime.Now.Ticks.ToString() + "_" + file.Name + ".txt";
                }
                string filePath = Path.Combine(uploadsFolder, pathFile);

                // Chuyển đổi tệp thành chuỗi Base64
                string base64String = ConvertVideoToBase64(file);

                // Lưu chuỗi Base64 vào ổ đĩa
                await File.WriteAllTextAsync(filePath, base64String);
            }

            return basePath+pathFile;
        }

        // convert video to base 64
        static string ConvertVideoToBase64(IFormFile file)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    byte[] videoBytes = memoryStream.ToArray();
                    string base64String = Convert.ToBase64String(videoBytes);
                    return base64String;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> UploadAvatarUser(IFormFile file)
        {
            string PathFile = string.Empty;
            string basePath = "/upload/avatar/";
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/avatar");

                if (file.FileName == null)
                    PathFile = "blank_avatar.png";
                else
                    PathFile = DateTime.Now.Ticks.ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, PathFile);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return basePath + PathFile;

        }

        public async Task<string> UploadBannerImg(IFormFile file)
        {
            string PathFile = string.Empty;
            string basePath = "/upload/banner/";
            if (file != null)
            {
                string uploadsFolder = Path.Combine(_iHostingEnvironment.ContentRootPath, "wwwroot/upload/banner");

                if (file.FileName == null)
                    PathFile = "banner1.png";
                else
                    PathFile = DateTime.Now.Ticks.ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, PathFile);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {   
                    await file.CopyToAsync(fileStream);
                }
            }
            return basePath + PathFile;

        }
    }


}

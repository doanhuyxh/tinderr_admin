namespace tinderr.Services
{
    public interface ICommon
    {
        string RandomString(int length);
        Task<string> UploadImgAvatarFilm (IFormFile file);
        Task<string> UploadVideoBase64 (IFormFile file);
        Task<string> UploadAvatarUser (IFormFile file);
        Task<string> UploadBannerImg (IFormFile file);
    }
}

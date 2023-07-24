namespace tinderr.Models.AccountViewModel
{
    public class EditUser
    {
        public string ApplicationUserId { get; set; }
        public string Name { get; set; }
        public string Bankname { get; set; }
        public string Banknumber { get; set; }
        public int Balance { get; set; }
        public string Role { get; set; }
        public IFormFile AvatarFile { get; set; }
    }
}

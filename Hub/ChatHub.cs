using Microsoft.AspNetCore.SignalR;
using tinderr.Data;
using tinderr.Models;

namespace tinderr.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly string adminChat;
        private readonly IConfiguration _configuration;
        public ChatHub(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            adminChat = _configuration["ChatCofing:AdminChat"]?? "supperadmin";
        }

        //user client gửi tin đến admin quản lý tin nhắn
        public async Task SendMessage(string user, string message)
        {
            if (!string.IsNullOrEmpty(user))
            {
                Message messages = new();
                messages.createDate = DateTime.Now;
                messages.ToUser = adminChat;
                messages.Status = false;
                messages.Content = message;
                messages.FromUser = user;
                await _context.AddAsync(messages);
                await _context.SaveChangesAsync();

            }

            //client và admin nhận lại sau khi gửi tới server
            await Clients.All.SendAsync("ReceiveMessageToUser", user, adminChat, message);
        }

        //Client của user gọi hàm này đầu tiền khi kết nối
        public async Task HistoryChat(string user)
        {
            // lấy dữ liệu chat của user đây
            var data = from ms in _context.Message
                       where ms.FromUser == user || ms.ToUser == user
                       select ms;

            var history = data.ToList();
            // trả về dữ liệu khi sau gọi đến
            await Clients.Caller.SendAsync("ReceiveMessageHistoryToUser", user, history);
        }

        //admin gửi tin nhắn
        public async Task AdminSendMessage(string user_nguoiDung, string admin, string mesage)
        {
            if (!string.IsNullOrEmpty(user_nguoiDung))
            {
                Message messages = new();
                messages.createDate = DateTime.Now;
                messages.ToUser = user_nguoiDung;
                messages.Content = mesage;
                messages.FromUser = admin;
                messages.Status = true;
                await _context.AddAsync(messages);
                await _context.SaveChangesAsync();
            }

            //client và admin nhận lại thông báo sau khi gửi tới server
            await Clients.All.SendAsync("ReceiveMessageToAdmin", user_nguoiDung, admin, mesage);
        }
        
        //lịch sử chat đơn của admin, hàm gọi sau khi admin kết nối lại
        public async Task HistoryChatAdmin(string admin)
        {
            var listuser = from us in _context.ApplicationUser
                           where us.IsActive == true
                           select new
                           {
                               adminUser = us.UserName,
                               adminName = us.Name,
                               adminAvatar = us.AvatartPath,
                           };
            
            //trả về danh sách client của admin
            await Clients.Caller.SendAsync("ReceiveMessageHistoryToAdmin", admin, listuser.ToList());
        }
    }
}

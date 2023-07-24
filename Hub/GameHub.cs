using Microsoft.AspNetCore.SignalR;
using tinderr.Data;


namespace tinderr.Hubs
{
    public class GameHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private static Random random = new Random();
        private readonly int countDown;
        private static Timer timer;

        public GameHub(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            countDown = int.Parse(_configuration["GameConfig:countDown"] ?? "60");
        }

        //    public override Task OnDisconnectedAsync(Exception exception)
        //    {
        //        // Dừng đồng hồ và giải phóng tài nguyên của nó khi một client mất kết nối
        //        StopTimer();
        //        return base.OnDisconnectedAsync(exception);
        //    }

        //    public async Task StartGameSession()
        //    {
        //        if (timer == null)
        //        {
        //            int remainingSeconds = countDown;

        //            timer = new Timer(1000); // 1000ms = 1 giây
        //            timer.Elapsed += async (sender, e) =>
        //            {
        //                if (remainingSeconds > 0)
        //                {
        //                    // Gửi số giây còn lại của đồng hồ đến tất cả các client
        //                    await Clients.All.SendAsync("CountdownUpdate", remainingSeconds);
        //                    remainingSeconds--;
        //                }
        //                else
        //                {
        //                    // Đồng hồ đã hết giờ, tạo và gửi một số ngẫu nhiên
        //                    int randomNumber = GenerateRandomNumber();
        //                    await Clients.All.SendAsync("CountdownFinished", randomNumber);

        //                    // Dừng đồng hồ và giải phóng tài nguyên của nó
        //                    StopTimer();
        //                }
        //            };

        //            timer.Start();
        //        }
        //    }


        //    private void StopTimer()
        //    {
        //        timer?.Stop();
        //        timer?.Dispose();
        //        timer = null;
        //    }


        //    private int GenerateRandomNumber()
        //    {
        //        return random.Next(1, 101); // Generate a random number from 1 to 100
        //    }
    }
}
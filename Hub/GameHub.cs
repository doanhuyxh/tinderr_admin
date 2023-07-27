using Microsoft.AspNetCore.SignalR;
using tinderr.Data;
using tinderr.Models;
using System.Threading.Tasks;

namespace tinderr.Hubs
{
    public class GameHub : Hub
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private int count { set; get; }
        private readonly int countDown;
        private static Timer timer;

        public GameHub(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            countDown = int.Parse(_configuration["GameConfig:countDown"] ?? "60");
        }

        public async Task CountDownUpDate(int count)
        {
            await Clients.All.SendAsync("UpdateCountDown", count);
        }
        
        public async Task CountDownFinish(int item1, int item2)
        {

            HistoryGame hs = new HistoryGame();
            hs.item1 = item2;
            hs.item2 = item2;
            hs.CreatedDate = DateTime.Now;
            hs.wave = DateTime.Now.Ticks.ToString();    
            hs.IsDeleted = false;
            _context.Add(hs);
            await _context.SaveChangesAsync();

            count = countDown;
            await Clients.All.SendAsync("CountDownFinishClient", item1, item2);
        }
        
    }
}
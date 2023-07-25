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
        
        public async Task CountDownFinish(bool random_1, bool random_2, bool random_3, bool random_4)
        {

            HistoryGame hs = new HistoryGame();
            hs.item1 = random_1;
            hs.item2 = random_2;
            hs.item3 = random_3;
            hs.item4 = random_4;
            hs.CreatedDate = DateTime.Now;
            hs.wave = DateTime.Now.Ticks.ToString();    
            hs.IsDeleted = false;
            _context.Add(hs);
            await _context.SaveChangesAsync();

            count = countDown;
            await Clients.All.SendAsync("CountDownFinishClient", random_1, random_2, random_2, random_2);
        }
        
    }
}
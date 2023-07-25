using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.ContentModel;
using tinderr.Models;

namespace tinderr.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<AseetVideo> AseetVideo { get; set; }
        public DbSet<RomChat> RomChat { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<MessageInRoom> MessageInRoom { get; set; }
        public DbSet<Banner> Banner { get; set; }
        public DbSet<HistoryGame> HistoryGame { get; set; }
        public DbSet<HistoryGameUser> HistoryGameUser { get; set; }
        public DbSet<Notify> Notify { get; set; }
        public DbSet<OtherUserChat> OtherUserChat { get; set; }
        public DbSet<HistoryBalance> HistoryBalance { get; set; }
        

        
    }
}
using Microsoft.EntityFrameworkCore;
using PlayerApi.Models;

namespace PlayerApi
{
    public class AudioPlayerContext : DbContext
    {
        public DbSet<Audio> Audio { get; set; }
        public DbSet<Performer> Performer { get; set; }
        public DbSet<Playlist> Playlist { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=LAPTOP-LMHEG0J6\SQLEXPRESS;Database=audioplayerdb;Trusted_Connection=True;");
        }
    }
}

using Microsoft.EntityFrameworkCore;
using PlayerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerApi
{
    public class AudioPlayerContext : DbContext
    {
        public DbSet<Audio> Audio { get; set; }
        public DbSet<Performer> Performer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=LAPTOP-LMHEG0J6\SQLEXPRESS;Database=audioplayerdb;Trusted_Connection=True;");
        }
    }
}

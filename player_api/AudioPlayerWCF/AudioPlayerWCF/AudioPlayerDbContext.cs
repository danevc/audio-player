using AudioPlayerWCF.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerWCF
{
    class AudioPlayerDbContext : DbContext
    {
        public AudioPlayerDbContext() 
            : base("ConnectionString") { }

        public DbSet<AudioDTO> Audio { get; set; }
    }
}

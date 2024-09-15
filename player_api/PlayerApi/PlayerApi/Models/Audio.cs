using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerApi.Models
{
    public class Audio
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Performer> Performer { get; set; } = new List<Performer>();
        public List<Playlist> Playlist { get; set; } = new List<Playlist>();
        public string Path { get; set; }
        public int Duration { get; set; }
        public DateTime CreationDate { get; set; }
        public int AmountAuditions { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace PlayerApi.Models
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Audio> Audio { get; set; } = new List<Audio>();
        public string CoverPath { get; set; }
    }
}

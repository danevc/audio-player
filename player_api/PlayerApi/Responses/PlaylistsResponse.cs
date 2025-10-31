using PlayerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerApi.Responses
{
    public class PlaylistsResponse
    {
        public List<Playlist> Playlists { get; set; }
        public bool Error { get; set; }
    }
}

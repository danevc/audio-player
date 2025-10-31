using PlayerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerApi.Responses
{
    public class SearchResultResponse
    {
        public List<Audio> Audios { get; set; }
        public List<Performer> Performers { get; set; }
    }
}

using PlayerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerApi.Responses
{
    public class AudioResponse
    {
        public int Count { get; set; }
        public List<Audio> Audios { get; set; }
        public Audio Audio { get; set; }
        public bool Error { get; set; }
    }
}

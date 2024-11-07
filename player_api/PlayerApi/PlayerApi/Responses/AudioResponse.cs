using PlayerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerApi.Responses
{
    public class AudioResponse
    {
        public List<Audio> Audios { get; set; }
        public int QuantityAudios { get; set; }

    }
}

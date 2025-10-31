using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerApi.Requests
{
    public class GetAudioRequest
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public string Query { get; set; }
        public int PlaylistId { get; set; }
    }
}

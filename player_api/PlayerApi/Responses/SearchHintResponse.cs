using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerApi.Responses
{
    public class SearchHintResponse
    {
        public List<string> Audios { get; set; }
        public List<string> Performers { get; set; }
    }
}

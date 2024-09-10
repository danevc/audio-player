using PlayerApi.Models;
using System.Collections.Generic;

namespace PlayerApi.Controllers
{
    public class PerformerResponse
    {
        public int Count { get; set; }
        public List<Performer> Performers { get; set; }
        public bool Error { get; set; }
    }
}
using PlayerApi.Models;
using System.Collections.Generic;

namespace PlayerApi.Controllers
{
    internal class PerformerResponse
    {
        public int Count { get; set; }
        public List<Performer> Performers { get; set; }
        public bool Error { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerApi.Models
{
    public class Performer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Audio> Audio { get; set; } = new List<Audio>();
    }
}

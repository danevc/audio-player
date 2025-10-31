using System.Collections.Generic;

namespace PlayerApi.Models
{
    public class Performer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Audio> Audio { get; set; } = new List<Audio>();
    }
}

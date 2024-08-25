using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerWCF.Dto
{
    public class AudioDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Performer { get; set; }

        public string Path { get; set; }

        public int Duration { get; set; }
    }
}

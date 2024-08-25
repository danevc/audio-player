using AudioPlayerWCF.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioPlayerWCF.Responses
{
    public class AudioResponse
    {
        public List<AudioDTO> AudioDTO { get; set; }

        public PerformerDTO Performer { get; set; }
    }
}

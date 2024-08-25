using AudioPlayerWCF.Dto;
using AudioPlayerWCF.Responses;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;


namespace AudioPlayerWCF
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class AudioPlayerService : IAudioPlayerService
    {
        using(var db = new audioplayerEntities())
        public AudioResponse GetAudios(int page, int limit)
        {
            var results = new List<AudioDTO>();
            var conString = @"data source=LAPTOP-LMHEG0J6\SQLEXPRESS;initial catalog=audioplayer;integrated security=True;";
            return null;
        }

        public int GetSum(int page, int limit)
        {
            return page + limit;
        }
    }
}

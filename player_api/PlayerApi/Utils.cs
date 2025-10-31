using PlayerApi.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace PlayerApi
{
    public static class Utils
    {
        public static readonly string _basePath = "D:\\Danil\\Медиатека\\Music\\";

        public static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            IgnoreNullValues = true,
            MaxDepth = 10
        };

        private static readonly string RusKey = "Ё!\"№;%:?*()_+ЙЦУКЕНГШЩЗХЪ/ФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,ё1234567890-=йцукенгшщзхъ\\фывапролджэячсмитьбю. ";
        private static readonly string EngKey = "~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./ ";

        public static List<Audio> AudiosNormalize(List<Audio> audios)
        {
            var res = new List<Audio>();
            List<Performer> performers;
            foreach (var a in audios)
            {
                performers = new List<Performer>();
                foreach (var p in a.Performer)
                {
                    performers.Add(new Performer
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Audio = null
                    });
                }
                res.Add(new Audio
                {
                    Id = a.Id,
                    Title = a.Title,
                    Path = a.Path,
                    Duration = a.Duration,
                    Performer = performers
                });
            }
            return res;
        }

        public static string EngToRus(string text)
        {
            string s = "";
            for (int i = 0; i < text.Length; i++)
            {
                try
                {
                    s += EngKey.Substring(RusKey.IndexOf(text[i]), 1);
                }
                catch
                {
                    s += RusKey.Substring(EngKey.IndexOf(text[i]), 1);
                }
            }
            return s;
        }
    }
}

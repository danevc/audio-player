using PlayerApi.Models;
using System.Collections.Generic;

namespace PlayerApi
{
    public static class Utils
    {
        private static readonly string RusKey = "Ё!\"№;%:?*()_+ЙЦУКЕНГШЩЗХЪ/ФЫВАПРОЛДЖЭЯЧСМИТЬБЮ,ё1234567890-=йцукенгшщзхъ\\фывапролджэячсмитьбю. ";
        private static readonly string EngKey = "~!@#$%^&*()_+QWERTYUIOP{}|ASDFGHJKL:\"ZXCVBNM<>?`1234567890-=qwertyuiop[]\\asdfghjkl;'zxcvbnm,./ ";

        /** FirstWriteBase
        public static void FirstWriteBase()
        {
            using (var db = new AudioPlayerContext())
            {
                var directoriesList = new List<string>();
                var filesList = new List<string>();
                string dirName = "D:\\Danil\\Music";
                string[] dirs = Directory.GetDirectories(dirName);
                foreach (string s in dirs)
                {
                    var dirInfo = new DirectoryInfo(s);
                    string[] files = Directory.GetFiles(s);

                    foreach (var f in files)
                    {
                        if (Path.GetExtension(f) == ".mp3")
                        {
                            var audioInfo = TagLib.File.Create(f);

                            var audio = new Audio
                            {
                                Title = audioInfo.Tag.Title,
                                Path = f.Substring(15),
                                Duration = Convert.ToInt32(Math.Round(audioInfo.Properties.Duration.TotalSeconds)),
                                CreationDate = DateTime.Now
                            };

                            foreach (var artist in audioInfo.Tag.Performers)
                            {
                                var perf = db.Performer.FirstOrDefault(e => e.Name == artist);
                                if (perf == null)
                                {
                                    var perfNew = new Performer { Name = artist };
                                    db.Performer.Add(perfNew);
                                    _logger.Trace($"ADD | {perfNew.GetType()} | Id: {perfNew.Id} Name: {perf.Name}");
                                    perfNew.Audio.Add(audio);
                                }
                                else
                                {
                                    perf.Audio.Add(audio);
                                    _logger.Trace($"ADD | {audio.GetType()} | Id: {audio.Id} Title: {audio.Title}");
                                }
                            }

                            db.SaveChanges();
                        }
                    }
                }
            }
        }*/

        public static List<Audio> AudioListNormalize(List<Audio> audios)
        {
            var audiosRes = new List<Audio>();
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
                audiosRes.Add(new Audio
                {
                    Id = a.Id,
                    Title = a.Title,
                    Path = a.Path,
                    Duration = a.Duration,
                    Performer = performers
                });
            }
            return audiosRes;
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

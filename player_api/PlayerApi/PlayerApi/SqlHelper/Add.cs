using NLog;
using PlayerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerApi.SqlHelper
{
    public static class Add
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static string Audio(string path)
        {
            var audioInfo = TagLib.File.Create(path);

            var audio = new Audio
            {
                Title = audioInfo.Tag.Title,
                Path = path.Substring(15),
                Duration = Convert.ToInt32(Math.Round(audioInfo.Properties.Duration.TotalSeconds)),
                CreationDate = DateTime.Now
            };
            using (var db = new AudioPlayerContext())
            {
                foreach (var artist in audioInfo.Tag.Performers)
                {
                    var perf = db.Performer.FirstOrDefault(e => e.Name == artist);
                    if (perf == null)
                    {
                        var perfNew = new Performer { Name = artist };
                        db.Performer.Add(perfNew);
                        //_logger.Trace($"Add | {perfNew.GetType()} with Name: {artist}");
                        perfNew.Audio.Add(audio);
                        //_logger.Trace($"Add | {audio.GetType()} with Title: {audio.Title}");
                    }
                    else
                    {
                        perf.Audio.Add(audio);
                        //_logger.Trace($"Add | {audio.GetType()} with Title: {audio.Title}");
                    }
                }

                db.SaveChanges();
            }
            return "";
        }

        public static string Playlist(string title = "", string coverPath = "", List<int> audioIds = null)
        {
            using (var db = new AudioPlayerContext())
            {
                if (string.IsNullOrEmpty(title))
                {
                    title = "Плейлист";
                }
                var playlist = new Playlist()
                {
                    Title = title,
                    CreationDate = DateTime.Now,
                    CoverPath = coverPath
                };
               
                db.Playlist.Add(playlist);
                if (audioIds != null && audioIds.Any())
                {
                    foreach (var id in audioIds)
                    {
                        var audio = db.Audio.FirstOrDefault(a => a.Id == id);
                        playlist.Audio.Add(audio);
                    }
                }

                db.SaveChanges();
                _logger.Trace($"Add | {playlist.GetType()} with Title: {playlist.Title}");
                return "";
            }
        }
    }
}

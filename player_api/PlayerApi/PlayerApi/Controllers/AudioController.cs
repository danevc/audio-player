using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayerApi.Responses;
using System.Collections.Generic;
using PlayerApi.Models;
using System.IO;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PlayerApi.Controllers
{
    [ApiController]
    public class AudioController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Action()
        {
            return Json("Server Audio started");
        }

        [HttpGet]
        [Route("GetAudiosPart")]
        public IActionResult GetAudiosPart(int page = 0, int limit = 10)
        {
            using (var db = new AudioPlayerContext())
            {
                var audio = db.Audio.OrderBy(c => c.Duration).Skip(limit * page).Take(limit).Include(a => a.Performer).ToList();
                var audios = new List<Audio>();
                List<Performer> performers;
                foreach (var a in audio)
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
                    audios.Add(new Audio
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Path = a.Path,
                        Duration = a.Duration,
                        Performer = performers
                    });
                    
                }
                var audioResponse = new AudioResponse
                {
                    Count = 2,
                    Audios = audios,
                    Error = false
                };
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true,
                    IgnoreNullValues= true,
                    MaxDepth = 10
                };
                
                return Json(audioResponse, jsonOptions);
            }
        }

        [HttpGet]
        [Route("GetAudio")]
        public IActionResult GetAudio(int id)
        {
            using (var db = new AudioPlayerContext())
            {
                var audio = db.Audio.Include(a => a.Performer).FirstOrDefault(e => e.Id == id);
                var performers = new List<Performer>();
                foreach (var p in audio.Performer)
                {
                    performers.Add(new Performer
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Audio = null
                    });
                }
                audio.Performer = performers;

                var audioResponse = new AudioResponse
                {
                    Audio = audio,
                    Error = false
                };

                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, 
                    WriteIndented = true,
                    IgnoreNullValues = true,
                    MaxDepth = 10
                };

                return Json(audioResponse, jsonOptions);
            }
        }

        [HttpGet]
        [Route("GetAudioFile")]
        public IActionResult GetAudioFile(int id)
        {
            using (var db = new AudioPlayerContext())
            {
                var audio = db.Audio.FirstOrDefault(e => e.Id == id);
                //var fs = new FileStream(audio.Path, FileMode.Open);
                //return File(mas, "audio/mpeg");
                using var fs = new FileStream(audio.Path, FileMode.Open, FileAccess.Read);
                using var br = new BinaryReader(fs);
                long numBytes = new FileInfo(audio.Path).Length;
                var buff = br.ReadBytes((int)numBytes);
                var fileResult = File(buff, "audio/mpeg");
                fileResult.EnableRangeProcessing = true;
                return fileResult;
            }
        }

        [HttpGet]
        [Route("GetPerformersPart")]
        public IActionResult GetPerformersPart(int page = 0, int limit = 10)
        {
            using (var db = new AudioPlayerContext())
            {
                var perf = new Performer
                {
                    Name = "Miley"
                };

                var performer = db.Performer.OrderBy(c => c.Name).Skip(limit * page).Take(limit).Include(a => a.Audio).ToList();
                var performers = new List<Performer>();
                List<Audio> audios;
                foreach (var p in performer)
                {
                    audios = new List<Audio>();
                    foreach (var a in p.Audio)
                    {
                        audios.Add(new Audio
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Path = a.Path,
                            Duration = a.Duration
                        });
                    }
                    performers.Add(new Performer
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Audio = audios
                    });

                }
                var performersResponse = new PerformerResponse
                {
                    Count = 2,
                    Performers = performers,
                    Error = false
                };
                var jsonOptions = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true,
                    IgnoreNullValues = true,
                    MaxDepth = 10
                };

                return Json(performersResponse, jsonOptions);
            }
        }

        [HttpGet]
        [Route("v")]
        public IActionResult FirstWriteBase()
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

                    if (dirInfo.Name[0] != '_')
                    {

                        var perf = new Performer { Name = dirInfo.Name };
                        db.Performer.Add(perf);

                        string[] files = Directory.GetFiles(s);

                        foreach (var f in files)
                        {
                            var audioInfo = TagLib.File.Create(f);
                            var audio = new Audio
                            {
                                Title = audioInfo.Tag.Title,
                                Path = f,
                                Duration = Convert.ToInt32(Math.Round(audioInfo.Properties.Duration.TotalSeconds))
                            };

                            perf.Audio.Add(audio);
                        }
                        //db.SaveChanges();
                    }
                }
                
                return Json(directoriesList);
            }
        }

    }
}

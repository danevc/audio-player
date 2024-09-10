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
using NLog;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;

namespace PlayerApi.Controllers
{
    public static class Utils
    {
        public static List<Audio> AudiosListNormalize(List<Audio> audios)
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
    }

    [ApiController]
    public class AudioController : Controller
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly string basePath = "D:\\Danil\\Music\\";
        private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            IgnoreNullValues = true,
            MaxDepth = 10
        };


        [HttpGet]
        [Route("")]
        public IActionResult Action()
        {
            return Json("start");
        }

        [HttpGet]
        [Route("GetAudiosPart")]
        public IActionResult GetAudiosPart(int page = 0, int limit = 10, string query = "")
        {
            using (var db = new AudioPlayerContext())
            {
                var audios = new List<Audio>();
                int quantityAudios;
                if(query == "")
                {
                    audios = db.Audio.OrderByDescending(a => a.AmountAuditions).Skip(limit * page).Take(limit).Include(a => a.Performer)
                        .ToList();
                    quantityAudios = db.Audio.Count();
                }
                else
                {
                    var audioByAudioSearch = db.Audio
                        .Where(a => EF.Functions.Like(a.Title, $"{query}%"))
                        .Include(a => a.Performer)
                        .ToList();
                    var performers = db.Performer
                        .Where(p => EF.Functions.Like(p.Name, $"{query}%"))
                        .Include(p => p.Audio)
                        .ToList();

                    var audioByPerfSearch = new List<Audio>();
                    quantityAudios = db.Audio.Where(a => EF.Functions.Like(a.Title, $"{query}%")).Count();
                    foreach (var p in performers)
                    {
                        foreach (var a in p.Audio)
                        {
                            audioByPerfSearch.Add(a);
                            quantityAudios++;
                        }
                    }
                    audios = audioByAudioSearch
                        .Union(audioByPerfSearch)
                        .OrderByDescending(a => a.AmountAuditions)
                        .Skip(limit * page)
                        .Take(limit)
                        .ToList();
                }
                
                var audioResponse = new AudioResponse
                {
                    Count = quantityAudios,
                    Audios = Utils.AudiosListNormalize(audios),
                    Error = false
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
                audio.AmountAuditions++;
                db.SaveChanges();
                var path = basePath + audio.Path;
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                using var br = new BinaryReader(fs);
                long numBytes = new FileInfo(path).Length;
                var buff = br.ReadBytes((int)numBytes);
                var fileResult = File(buff, "audio/mpeg");
                fileResult.EnableRangeProcessing = true;
                return fileResult;
            }
        }

        [HttpGet]
        [Route("SearchHint")]
        public IActionResult SearchHint(string query)
        {
            using (var db = new AudioPlayerContext())
            {
                var take = 8;
                var audios = db.Audio.Where(a => EF.Functions.Like(a.Title, $"{query}%")).OrderBy(a => a.Title).Select(a => a.Title).Take(take).ToList();
                var performers = db.Performer.Where(a => EF.Functions.Like(a.Name, $"{query}%")).OrderBy(a => a.Name).Select(a => a.Name).Take(take).ToList();

                var hintResponse = new SearchHintResponse()
                {
                    Audios = audios,
                    Performers = performers
                };
                
                return Json(hintResponse, jsonOptions);
            }
        }

        [HttpGet]
        [Route("SearchResult")]
        public IActionResult SearchResult(string query)
        {
            using (var db = new AudioPlayerContext())
            {
                var take = 20;
                var audioByAudioSearch = db.Audio.Where(a => EF.Functions.Like(a.Title, $"{query}%")).Include(a => a.Performer).Take(take).ToList();
                var perf = db.Performer.Where(p => EF.Functions.Like(p.Name, $"{query}%")).Select(a => a.Id).Take(take).ToList();
                var audioByPerfSearch = new List<Audio>();
                foreach (var id in perf)
                {
                    audioByPerfSearch.Union(db.Audio.Where(p => p.Id == id));
                }
                var audio = audioByAudioSearch.Union(audioByPerfSearch);

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

                return Json(audioResponse, jsonOptions);
            }
        }

        /**[HttpGet]
        [Route("SearchResult")]
        public IActionResult SearchResult(string query, int page = 0, int limit = 10)
        {
            using (var db = new AudioPlayerContext())
            {
                var audio = db.Audio.Where(a => EF.Functions.Like(a.Title, $"{query}%") || EF.Functions.Like(a.Title, $"[ ]{query}%"))
                    .Include(a => a.Performer).Skip(limit * page).Take(limit).OrderBy(a => a.Title);
                //var audioSearch2 = db.Audio.Where(a => EF.Functions.Like(a.Title, $"[ ]{query}%")).Include(a => a.Performer);
                //var audioSearch3 = db.Audio.Where(a => EF.Functions.Like(a.Title, $"%{query}%")).Include(a => a.Performer);

                //var audioSearchResult = audioSearch1.Skip(limit * page).Take(limit);
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
                return Json(audioResponse, jsonOptions);
            }
        }
        */


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
                    string[] files = Directory.GetFiles(s);
                    
                    foreach (var f in files)
                    {
                        if(Path.GetExtension(f) == ".mp3")
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
                                if(perf == null)
                                {
                                    var perfNew = new Performer { Name = artist };
                                    db.Performer.Add(perfNew);
                                    perfNew.Audio.Add(audio);
                                }
                                else
                                {
                                    perf.Audio.Add(audio);
                                }
                            }

                            db.SaveChanges();
                        }
                    }
                }
                return Json(directoriesList);
            }
        }

    }
}

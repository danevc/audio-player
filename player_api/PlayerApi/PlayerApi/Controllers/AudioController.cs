using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayerApi.Responses;
using System.Collections.Generic;
using PlayerApi.Models;
using System.IO;
using System;
using System.Threading.Tasks;
using NLog;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using PlayerApi.SqlHelper;

namespace PlayerApi.Controllers
{
    [ApiController]
    public class AudioController : Controller
    {
        private static readonly string _basePath = "D:\\Danil\\Music\\";
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
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
            /**
            var ids = new List<int>();
            for (int i = 6179; i < 6238; i++)
            {
                ids.Add(i);
            }
            Add.Playlist("Well", "__pics\\8c1785c093c67382c1d92916ea2c4419.jpg", ids);
           */

            return Ok();
        }

        [HttpPost]
        [Route("AddAudios")]
        public async Task<IActionResult> AddAudios(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                string path = string.Format(@"{0}__Files__\\{1}.mp3", _basePath, DateTime.Now.Ticks);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                Add.Audio(path);
            }
            return Ok();
        }

        [HttpGet]
        [Route("GetAudiosPart")]
        public IActionResult GetAudiosPart(int page = 0, int limit = 10, string query = "", int playlistId = -1)
        {
            using (var db = new AudioPlayerContext())
            {
                var audios = new List<Audio>();
                int quantityAudios;
                if(String.IsNullOrEmpty(query))
                {
                    audios = db.Audio.OrderByDescending(a => a.CreationDate).Skip(limit * page).Take(limit).Include(a => a.Performer)
                        .ToList();
                    quantityAudios = db.Audio.Count();
                }
                else
                {
                    audios = SQLSelector.SearchAudios(query, page, limit);
                    if (!audios.Any())
                    {
                        var queryInvert = Utils.EngToRus(query);
                        audios = SQLSelector.SearchAudios(queryInvert, page, limit);
                    }
                    quantityAudios = SQLSelector.SearchAudiosCount(query);
                }

                if (playlistId != -1) {
                    audios = SQLSelector.AudiosByPlaylist(playlistId, page, limit);
                    if (!audios.Any())
                    {
                        var queryInvert = Utils.EngToRus(query);
                        audios = SQLSelector.SearchAudios(queryInvert, page, limit);
                    }
                    quantityAudios = SQLSelector.AudiosByPlaylistCount(playlistId);
                }


                var audioResponse = new AudioResponse
                {
                    Count = quantityAudios,
                    Audios = Utils.AudioListNormalize(audios),
                    Error = false
                };
                
                return Json(audioResponse, _jsonOptions);
            }
        }

        [HttpGet]
        [Route("GetPlaylistsPart")]
        public IActionResult GetPlaylistsPart(int page = 0, int limit = 5, string query = "")
        {
            using (var db = new AudioPlayerContext())
            {
                var playlists = new List<Playlist>();
                if (query == "")
                {
                    playlists = db.Playlist.OrderByDescending(p => p.CreationDate).Skip(limit * page).Take(limit)
                        .ToList();
                }

                var playlistsResponse = new PlaylistsResponse
                {
                    Playlists = playlists,
                    Error = false
                };

                return Json(playlistsResponse, _jsonOptions);
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

                return Json(audioResponse, _jsonOptions);
            }
        }

        [HttpGet]
        [Route("GetPlaylist")]
        public IActionResult GetPlaylist(int id)
        {
            using (var db = new AudioPlayerContext())
            {
                var playlist = db.Playlist.FirstOrDefault(e => e.Id == id);

                if(playlist == null)
                {
                    return NotFound("Ресурс в приложении не найден");
                }

                return Json(playlist, _jsonOptions);
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
                var path = _basePath + audio.Path;
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
                var audios = db.Audio.Where(a => EF.Functions.Like(a.Title, $"{query}%")).OrderBy(a => a.Title).Select(a => a.Title).Distinct().Take(take).ToList();
                var performers = db.Performer.Where(a => EF.Functions.Like(a.Name, $"{query}%")).OrderBy(a => a.Name).Select(a => a.Name).Distinct().Take(take).ToList();

                var hintResponse = new SearchHintResponse()
                {
                    Audios = audios,
                    Performers = performers
                };
                
                return Json(hintResponse, _jsonOptions);
            }
        }

        [HttpGet]
        [Route("GetPlaylistCover")]
        public IActionResult GetCover(int id)
        {
            using (var db = new AudioPlayerContext())
            {
                var playlist = db.Playlist.FirstOrDefault(e => e.Id == id);
                var path = _basePath + playlist.CoverPath;
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                using var br = new BinaryReader(fs);
                long numBytes = new FileInfo(path).Length;
                var buff = br.ReadBytes((int)numBytes);
                var fileResult = File(buff, "image/jpeg");
                fileResult.EnableRangeProcessing = true;
                return fileResult;
            }
        }

        [HttpGet]
        [Route("GetAudiosByPlaylist")]
        public IActionResult GetAudiosByPlaylist(int playlistId, int page = 0, int limit = 10)
        {
            using (var db = new AudioPlayerContext())
            {
                var audios = new List<Audio>();
                int quantityAudios = 2;
                var playlist = db.Playlist.Include(a => a.Audio).ThenInclude(a => a.Performer).FirstOrDefault(e => e.Id == playlistId); 

                playlist.Audio = Utils.AudioListNormalize(playlist.Audio);

                var audioResponse = new AudioResponse
                {
                    Count = quantityAudios,
                    Audios = Utils.AudioListNormalize(audios),
                    Error = false
                };

                return Json(audioResponse, _jsonOptions);
            }
        }

        [HttpGet]
        [Route("DeletePlaylist")]
        public IActionResult DeletePlaylist(int playlistId)
        {
            using (var db = new AudioPlayerContext())
            {
                var p = db.Playlist.Find(playlistId);
                db.Playlist.Remove(p);
                db.SaveChanges();
                return Json("", _jsonOptions);
            }
        }

    }
}

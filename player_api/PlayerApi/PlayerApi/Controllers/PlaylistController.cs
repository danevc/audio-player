using Microsoft.AspNetCore.Mvc;
using System.Linq;
using PlayerApi.Responses;
using System.IO;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using PlayerApi.Models;

namespace PlayerApi.Controllers
{
    [ApiController]
    public class PlaylistController : Controller
    {
        [HttpGet]
        [Route("{Playlist}/GetPlaylists")]
        public IActionResult GetPlaylists(int page = 0, int limit = 5)
        {
            try
            {
                using (var db = new AudioPlayerContext())
                {
                    var playlists = db.Playlist.OrderByDescending(p => p.CreationDate).Skip(limit * page).Take(limit).ToList();

                    var playlistsResponse = new PlaylistsResponse
                    {
                        Playlists = playlists,
                        Error = false
                    };

                    return Json(playlistsResponse, Utils._jsonOptions);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{Playlist}/GetPlaylist")]
        public IActionResult GetPlaylist(int id)
        {
            try
            {
                using (var db = new AudioPlayerContext())
                {
                    var playlist = db.Playlist.FirstOrDefault(e => e.Id == id);

                    if (playlist == null)
                    {
                        return NotFound("Плейлист не найден");
                    }

                    return Json(playlist, Utils._jsonOptions);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("{Playlist}/CreatePlaylist")]
        public IActionResult CreatePlaylist([FromBody] CreatePlaylistRequest request)
        {
            try
            {
                using (var db = new AudioPlayerContext())
                {
                    var playlist = new Playlist()
                    {
                        Title = request.title,
                        Description = request.description
                    };

                    if (playlist == null)
                    {
                        return NotFound("Плейлист не найден");
                    }

                    return Json(playlist, Utils._jsonOptions);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{Playlist}/GetAudiosByPlaylist")]
        public IActionResult GetAudiosByPlaylist(int id, int page = 0, int limit = 10)
        {
            try
            {
                using (var db = new AudioPlayerContext())
                {
                    var audios = SQLSelector.AudiosByPlaylist(id, page, limit);
                    var quantityAudios = SQLSelector.AudiosByPlaylistCount(id);

                    var audioResponse = new AudioResponse
                    {
                        QuantityAudios = quantityAudios,
                        Audios = Utils.AudiosNormalize(audios),
                    };

                    return Json(audioResponse, Utils._jsonOptions);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{Playlist}/DeletePlaylist")]
        public IActionResult DeletePlaylist(int id)
        {
            try
            {
                using (var db = new AudioPlayerContext())
                {
                    var p = db.Playlist.Find(id);

                    if(p == null)
                    {
                        return NotFound("Плейлист не найден");
                    }

                    db.Playlist.Remove(p);
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{Playlist}/GetPlaylistCover")]
        public IActionResult GetPlaylistCover(int id)
        {
            try
            {
                using (var db = new AudioPlayerContext())
                {
                    var playlist = db.Playlist.FirstOrDefault(e => e.Id == id);

                    if (playlist == null)
                    {
                        return NotFound();
                    }

                    var path = Utils._basePath + playlist.CoverPath;

                    if (!System.IO.File.Exists(path))
                    {
                        throw new Exception("Файл по указанному пути не существует.");
                    }

                    using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    using var br = new BinaryReader(fs);
                    long numBytes = new FileInfo(path).Length;
                    var buff = br.ReadBytes((int)numBytes);
                    var fileResult = File(buff, "image/jpeg");
                    fileResult.EnableRangeProcessing = true;
                    return fileResult;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("{Playlist}/SetTitle")]
        public IActionResult SetTitle([FromBody] SetValueRequest request)
        {
            try
            {
                using (var db = new AudioPlayerContext())
                {
                    var playlist = db.Playlist.Find(request.id);

                    if(playlist == null)
                    {
                        return NotFound();
                    }

                    playlist.Title = request.value;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("{Playlist}/SetDescription")]
        public IActionResult SetDescription([FromBody] SetValueRequest request)
        {
            try
            {
                using (var db = new AudioPlayerContext())
                {
                    var playlist = db.Playlist.Find(request.id);

                    if (playlist == null)
                    {
                        return NotFound();
                    }

                    playlist.Description = request.value;
                    db.SaveChanges();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class SetValueRequest
    {
        public int id { get; set; }
        public string value { get; set; }
    }

    public class CreatePlaylistRequest
    {
        public string title { get; set; }
        public string description { get; set; }
        IFormFile cover { get; set; }
    }
}

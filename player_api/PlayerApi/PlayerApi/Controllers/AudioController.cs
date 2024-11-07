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
        [HttpGet]
        [Route("{Audio}/GetAudios")]
        public IActionResult GetAudios(int limit = 0, int page = 0)
        {
            try
            {
                using (var db = new AudioPlayerContext())
                {
                    var audios = new List<Audio>();
                    int quantityAudios;
                    audios = db.Audio.OrderByDescending(a => a.CreationDate).Skip(limit * page).Take(limit).Include(a => a.Performer)
                        .ToList();
                    quantityAudios = db.Audio.Count();

                    var audioResponse = new AudioResponse
                    {
                        QuantityAudios = quantityAudios,
                        Audios = Utils.AudiosNormalize(audios)
                    };

                    return Json(audioResponse, Utils._jsonOptions);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("{Audio}/AddAudios")]
        public async Task<IActionResult> AddAudios(List<IFormFile> files)
        {
            foreach (var file in files)
            {
                string path = string.Format(@"{0}__Files__\\{1}.mp3", Utils._basePath, DateTime.Now.Ticks);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    //await file.CopyToAsync(fileStream);
                }
                //Add.Audio(path);
            }
            return Ok();
        }

        [HttpGet]
        [Route("{Audio}/GetAudioById")]
        public IActionResult GetAudioById(int id)
        {
            try
            {
                using (var db = new AudioPlayerContext())
                {
                    var audio = db.Audio.Include(a => a.Performer).FirstOrDefault(e => e.Id == id);

                    if (audio == null)
                    {
                        return NotFound();
                    }

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

                    return Json(audio, Utils._jsonOptions);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{Audio}/GetAudioFile")]
        public IActionResult GetAudioFile(int id)
        {
            try
            {
                using (var db = new AudioPlayerContext())
                {
                    var audio = db.Audio.FirstOrDefault(e => e.Id == id);

                    if (audio == null)
                    {
                        return NotFound();
                    }

                    audio.AmountAuditions++;
                    db.SaveChanges();
                    var path = Utils._basePath + audio.Path;

                    if (!System.IO.File.Exists(path))
                    {
                        throw new Exception("Файл по указанному пути не существует.");
                    }

                    using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    using var br = new BinaryReader(fs);
                    long numBytes = new FileInfo(path).Length;
                    var buff = br.ReadBytes((int)numBytes);
                    var fileResult = File(buff, "audio/mpeg");
                    fileResult.EnableRangeProcessing = true;
                    return fileResult;
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }        
}

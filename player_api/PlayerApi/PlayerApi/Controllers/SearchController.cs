using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PlayerApi.Responses;
using System.Collections.Generic;
using System;

namespace PlayerApi.Controllers
{
    [ApiController]
    public class SearchController : Controller
    {
        [HttpGet]
        [Route("{Search}/GetAudiosSearchResult")]
        public IActionResult GetAudiosSearchResult(string query = "", int limit = 10, int page = 0)
        {
            try
            {
                var audios = SQLSelector.SearchAudios(query, page, limit);
                if (!audios.Any())
                {
                    var queryInvert = Utils.EngToRus(query);
                    audios = SQLSelector.SearchAudios(queryInvert, page, limit);
                }
                var quantityAudios = SQLSelector.SearchAudiosCount(query);

                var audioResponse = new AudioResponse
                {
                    QuantityAudios = quantityAudios,
                    Audios = Utils.AudiosNormalize(audios),
                };

                return Json(audioResponse, Utils._jsonOptions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("{Search}/SearchHint")]
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

                return Json(hintResponse, Utils._jsonOptions);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace PlayerApi.Controllers
{
    [ApiController]
    public class HomeController : Controller
    {
        [Route("")]
        public IActionResult Action()
        {
            return Ok();
        }
    }
}

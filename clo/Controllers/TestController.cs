using Microsoft.AspNetCore.Mvc;

namespace clo.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("~/Test/{action}")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace BrowserGameBackend.Controllers
{
    [Route("")]
    public class DebugPageController : Controller
    {
        public IActionResult DebugPage()
        {
            return View();
        }
    }
}

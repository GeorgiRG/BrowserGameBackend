using BrowserGameBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace BrowserGameBackend.Controllers.API
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult> test(User user)
        {
            if (true)
            {
                Console.WriteLine(user.Name);
                Console.WriteLine(user.Email);
                Console.WriteLine(user.Password);
                return Ok();
            }
        }
    }
}

using BrowserGameBackend.Models;
using BrowserGameBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrowserGameBackend.Controllers.API
{
    [Route("login")]
    [ApiController]
    public class LoginLogoutController : ControllerBase
    {
        private readonly Services.IAuthenticationService _authenticationService;
        private readonly ISessionService _sessionService;
        public CookieOptions cookieOptions = new()
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true
        };

        public LoginLogoutController(Services.IAuthenticationService authenticationService, ISessionService sessionService)
        {
            _authenticationService = authenticationService;
            _sessionService = sessionService;
        }
        
        [HttpGet]
        public async Task<IActionResult> LoginWithSession()
        {
            //if user picked remember me
            string? sessionId = Request.Cookies["sessionId"];
            Console.WriteLine(sessionId);

            if (await _sessionService.SessionIsStored(sessionId!))
            {
                string newSessionId = await _sessionService.CreateOrRefreshSession(sessionId: sessionId, rememberMe: true)!;
                if (newSessionId != null)
                {
                    cookieOptions.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Append("sessionId", $"{newSessionId}", cookieOptions);
                    return Ok();
                }
                else return BadRequest("Something went wrong, please try again");

            }
            else
            {
                Response.Cookies.Delete("sessionId");
                return BadRequest("Expired session");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user, bool rememberMe = false)
        {
            //regular login
            if (user == null) return BadRequest("No user provided");
            //checks fully login credentials
            string loginResult = await _authenticationService.Login(user.Email!, user.Password!);
            if (loginResult != "Ok") return BadRequest(loginResult);
            //sessionId
            string newSessionId = await _sessionService.CreateOrRefreshSession(email: user.Email, rememberMe: rememberMe)!;
            Response.Cookies.Append("sessionId", $"{newSessionId}", cookieOptions);
            return Ok();
            
        }
    }
    
}

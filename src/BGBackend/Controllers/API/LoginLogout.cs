using BrowserGameBackend.Dto;
using BrowserGameBackend.Models;
using BrowserGameBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BrowserGameBackend.Controllers.API
{
    [Route("login")]
    [ApiController]
    public class LoginLogoutController : ControllerBase
    {
        private readonly Services.IAuthenticationService _authenticationService;
        private readonly IUserManagementService _userManagementService;
        public CookieOptions cookieOptions = new()
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
            IsEssential= true,
        };

        public LoginLogoutController(
            Services.IAuthenticationService authenticationService,
            IUserManagementService userManagementService)
        {
            _authenticationService = authenticationService;
            _userManagementService = userManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> LoginWithSession()
        {
            //if user picked remember me
            string? sessionId = Request.Cookies["sessionId"];

            if (sessionId != null && sessionId.Length < 15)
            {
                UserDto? userDto = await _userManagementService.LoginUser(sessionId: sessionId, rememberMe: true)!;
                if (userDto != null)
                {
                    cookieOptions.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Append("sessionId", $"{userDto.SessionId}", cookieOptions);
                    return Ok(userDto);
                }
                else return BadRequest("Something went wrong, please try again");
            }
            else
            {
                Response.Cookies.Delete("sessionId");
                return Unauthorized("Expired session");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User user, bool rememberMe = false)
        {
            if (user == null) return BadRequest("No user provided");
            //checks fully the login credentials
            string loginResult = await _authenticationService.Login(user.Email!, user.Password!);
            if (loginResult != "Ok") return BadRequest(loginResult);
            //return logged user
            UserDto userDto = await _userManagementService.LoginUser(email: user.Email, rememberMe: rememberMe)!;
            if (userDto != null)
            {
                if (rememberMe)
                {
                    cookieOptions.Expires = DateTime.Now.AddYears(1);
                }
                Response.Cookies.Append("sessionId", $"{userDto.SessionId}", cookieOptions);
                return Ok(userDto);
            }
            else return BadRequest("No user with such email was found");
        }
    }
    
}

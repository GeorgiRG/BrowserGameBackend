using Microsoft.AspNetCore.Mvc;
using BrowserGameBackend.Models;
using BrowserGameBackend.Services;
using System.ComponentModel.DataAnnotations;
using BrowserGameBackend.Types;
using System.Security.Cryptography;

namespace BrowserGameBackend.Controllers.API
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Services.IAuthenticationService _authenticationService;
        private readonly IUserRegistrationService _userRegistration;
        private readonly ISessionService _sessionService;

        public UsersController(IUserRegistrationService userRegistration, Services.IAuthenticationService authenticationService, ISessionService sessionService)
        {
            _userRegistration = userRegistration;
            _authenticationService = authenticationService;
            _sessionService = sessionService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(string? sessionId, string? email)
        {
            try
            {
                string smth = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

                Console.WriteLine(Request.Cookies["sessionId"]);
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    Path = "/",
                    HttpOnly= true,
                    SameSite = SameSiteMode.None,
                    Secure = true
                };
                Response.Cookies.Append("sessionId", $"{smth}", cookieOptions);



                return Ok();


            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest();

            }
        }

        [Route("check")]
        [HttpGet]
        public async Task<IActionResult> CheckExisting(string? username, string? email)
        {
            bool result;

            if (username != null)
            {
                result = await _authenticationService.UsernameExists(username);
                return Ok(result);
            }
            else if (email != null)
            {
                result = await _authenticationService.EmailExists(email);
                return Ok(result);
            }
            else 
            { 
                return BadRequest(Response.Headers); 
            }
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody, Required] User user)
        {
            GeneralResponse response = new ();
            Console.WriteLine("{0}, {1}, {2}", user.Name, user.Email, user.Password);
            Console.WriteLine("\ndoing stuff\n");
            string result = await _userRegistration.CreateUser(user);
            if (result != "Ok") 
            {
                response.Message = "There were errors in user creation";
                response.Errors = result ?? "Bad input";
                return BadRequest(response) ;
            }
     
            response.Message = "User created successfully";
            return Ok(response);
        }

        [Route("confirmEmail/{confirmationCode}")]
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([Required] string confirmationCode)
        {
            Console.WriteLine("works");
            GeneralResponse response = new ();
            string result = await _userRegistration.ConfirmEmail(confirmationCode);
            if (result == null || result != "Ok")
            {
                response.Message = "There were errors in email confirmation";
                response.Errors = result ?? "Bad input";
                return BadRequest(response);
            }
            response.Message = "Email confirmed";
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUnconfirmed([FromBody, Required] User user)
        {

            return Ok();
        }
    }
}
        
using Microsoft.AspNetCore.Mvc;
using BrowserGameBackend.Models;
using BrowserGameBackend.Services;
using System.ComponentModel.DataAnnotations;
using BrowserGameBackend.Types;
using System.Security.Cryptography;
using BrowserGameBackend.Dto;

namespace BrowserGameBackend.Controllers.API
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Services.IAuthenticationService _authenticationService;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUserManagementService _userManagementService;

        public UsersController(IUserRegistrationService userRegistrationService, Services.IAuthenticationService authenticationService, IUserManagementService userManagementService)
        {
            _userRegistrationService = userRegistrationService;
            _authenticationService = authenticationService;
            _userManagementService = userManagementService;
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
                result = await _authenticationService.UsernameValidAndOriginal(username);
                return Ok(result);
            }
            else if (email != null)
            {
                result = await _authenticationService.EmailValidAndOriginal(email);
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
            string result = await _userRegistrationService.CreateUser(user);
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
            GeneralResponse response = new ();
            string result = await _userRegistrationService.ConfirmEmail(confirmationCode);
            if (result == null || result != "Ok")
            {
                response.Message = "There were errors in email confirmation";
                response.Errors = result ?? "Bad input";
                return BadRequest(response);
            }
            response.Message = "Email confirmed";
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCharacter(string? email = null, string? faction = null, string? species = null, string? password = null)
        {
            string? sessionId = Request.Cookies["sessionId"];
            UserDto? userDto = await _userManagementService.UpdateUser(sessionId!, email, faction, species, password)!;
            if (userDto == null) return BadRequest("Invalid session or input");
            return Ok(userDto);
        }
        
        [Route("levelUp")]
        [HttpPut]
        public async Task<IActionResult> LevelUp([Required, FromBody] UserSkills userSkills)
        {
            string? sessionId = Request.Cookies["sessionId"];
            UserDto? userDto = await _userManagementService.GetUserDto(sessionId: sessionId)!;
            if (userDto == null) return Unauthorized("Session expired");
            UserSkills? updatedUserSkills = await _userManagementService.UpdateUserSkills(userDto, userSkills)!;
            if (updatedUserSkills == null) return BadRequest("Invalid input");
            return Ok(updatedUserSkills);
        }

        [Route("{id}/colony")]
        [HttpPut]
        public async Task<IActionResult> UpdateToColony()
        {
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUnconfirmed([FromBody, Required] User user)
        {

            return Ok();
        }
    }
}
        
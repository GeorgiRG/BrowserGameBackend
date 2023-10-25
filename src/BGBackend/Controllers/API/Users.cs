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
        public CookieOptions cookieOptions = new()
        {
            Expires = DateTime.Now.AddDays(1),
            Path = "/",
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true
        };

        public UsersController(IUserRegistrationService userRegistrationService, Services.IAuthenticationService authenticationService, IUserManagementService userManagementService)
        {
            _userRegistrationService = userRegistrationService;
            _authenticationService = authenticationService;
            _userManagementService = userManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSelf(string? sessionId, string? email)
        {
            try
            {
                UserDto? userDto = await _userManagementService.GetUserDto(sessionId, email)!;
                if (userDto != null)
                {
                    return Ok(userDto);
                }
                else
                {
                    return NotFound();
                }


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
        public async Task<IActionResult> Register([FromBody, Required] UserRegistrationDto userData)
        {
            Console.WriteLine("{0}, {1}, {2}", userData.Name, userData.Email, userData.Password);
            string result = await _userRegistrationService.CreateUser(userData);
            if (result != "Ok") 
            {
                return BadRequest("Bad input") ;
            }

            //user input is already checked on registering
            UserDto userDto = await _userManagementService.LoginUser(email: userData.Email, rememberMe: false)!;
            if (userDto != null)
            {
                Response.Cookies.Append("sessionId", $"{userDto.SessionId}", cookieOptions);
                return Ok();
            }
            else
            {
                return BadRequest("Could not login user");
            }
        }

        [Route("confirmEmail")]
        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string confirmationCode)
        {
            Console.WriteLine($"Confirm email: {confirmationCode}");    
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
        public async Task<IActionResult> UpdateCharacter(string faction, string species)
        {
            string? sessionId = Request.Cookies["sessionId"];
            if (!await _authenticationService.SessionIsValid(sessionId!)) 
            {
                return Unauthorized();
            }
            UserDto? userDto = await _userManagementService.CharacterCreation(sessionId!, faction, species)!;
            if (userDto == null) return BadRequest("Invalid input");
            return Ok(userDto);
        }
        
        [Route("levelUp")]
        [HttpPut]
        public async Task<IActionResult> LevelUp([Required, FromBody] LevelUpSkillsDto userSkills)
        {
            Console.WriteLine(userSkills.SpaceWarfare + " works");
            string? sessionId = Request.Cookies["sessionId"];
            if (!await _authenticationService.SessionIsValid(sessionId!))
                return Unauthorized();
            
            UserSkills? updatedUserSkills = await _userManagementService.UpdateUserSkills(sessionId!, userSkills)!;
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
        
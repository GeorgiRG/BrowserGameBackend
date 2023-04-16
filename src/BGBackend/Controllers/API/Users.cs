using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Xml.Linq;
using System.Web;
using Microsoft.AspNetCore.Cors;
using BrowserGameBackend.Models;
using BrowserGameBackend.Services;
using System.ComponentModel.DataAnnotations;
using BrowserGameBackend.Types;

namespace BrowserGameBackend.Controllers.API
{
    [Route("users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRegistrationService _userRegistration;

        public UsersController(IUserRegistrationService userRegistration)
        {
            _userRegistration = userRegistration;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody, Required] User user)
        {
            GeneralResponse response = new ();
            string result = await _userRegistration.CreateUser(user);
            if (result != "Ok") 
            {
                response.Message = "There were errors in user creation";
                response.Errors = result ?? "Bad input";
                return BadRequest(response);
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
            GeneralResponse response = new();
            string result = await _userRegistration.CreateUser(user);
            if (result != "Ok")
            {
                response.Message = "There were errors in user creation";
                response.Errors = result ?? "Bad input";
                return BadRequest(response);
            }
            response.Message = "User created successfully";
            return Ok(response);
        }
    }
}
        
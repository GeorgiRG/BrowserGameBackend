using BrowserGameBackend.Models;
using Microsoft.AspNetCore.Mvc;
using BrowserGameBackend.Tools;
using BrowserGameBackend.Services;
using Microsoft.AspNetCore.Authorization;

namespace BrowserGameBackend.Controllers.API
{
    [Route("api/testing")]
    [ApiController]
    public class Testing : ControllerBase
    {
        private readonly IEmailService _emailService;

        public Testing(IEmailService emailService) 
        {
            _emailService= emailService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Test()
        {
            Console.WriteLine("Working");
            string input = "abcd";
            
            string result = PasswordTools.Hash(input);
            Console.WriteLine(result, PasswordTools.Verify(input, result));
            Console.WriteLine(PasswordTools.Verify(input, result));

            return Ok();
        }

        [HttpGet("email")]
        public async Task<ActionResult> Testmail()
        {
            Console.WriteLine("Woorks");
            await _emailService.Send("grgeorgi93@gmail.com", "Hello", "This is a test email again!");
            return Ok();
        }

        [HttpGet("validation")]
        public async Task<ActionResult> Testvalidation()
        {
            return Ok();
        }
    }
}

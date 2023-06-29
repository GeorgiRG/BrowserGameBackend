using BrowserGameBackend.Models;
using Microsoft.AspNetCore.Mvc;
using BrowserGameBackend.Tools;
using BrowserGameBackend.Services;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;

namespace BrowserGameBackend.Controllers.API
{
    [Route("api/testing")]
    [ApiController]
    public class Testing : ControllerBase
    {
        private readonly IEmailService _emailService;

        public Testing(IEmailService emailService, IGalaxyMapService galaxyMapService) 
        {
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult> Test()
        {
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

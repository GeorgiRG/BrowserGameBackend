using BrowserGameBackend.Models;
using Microsoft.AspNetCore.Mvc;
using BrowserGameBackend.Tools;
using BrowserGameBackend.Services;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using BrowserGameBackend.Services.Game;
using BrowserGameBackend.Enums;

namespace BrowserGameBackend.Controllers.API
{
    [Route("api/testing")]
    [ApiController]
    public class Testing : ControllerBase
    {
        private readonly IEmailService _emailService;

        public Testing(IEmailService emailService) 
        {
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult> Test()
        {
            Factions factions = new ();
            Dictionary<int, string> asdf = factions.Enumeration;
            foreach(var pair in asdf) { Console.WriteLine(pair.Key + "=" + pair.Value); }
            return Ok();
        }

        [HttpGet("email")]
        public async Task<ActionResult> Testmail()
        {
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

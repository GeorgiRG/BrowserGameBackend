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
        private readonly IGalaxyMapService _galaxyMapService;

        public Testing(IEmailService emailService, IGalaxyMapService galaxyMapService) 
        {
            _emailService= emailService;
            _galaxyMapService = galaxyMapService;
        }

        [HttpGet]
        public async Task<ActionResult> Test()
        {
            await _galaxyMapService.GenerateGalaxy();
            await _galaxyMapService.GeneratePlanets();
            await _galaxyMapService.GenerateBots();
            if (await _galaxyMapService.SettleFaction("Solar Empire"))
            {
                if (await _galaxyMapService.SettleFaction("Vega Legion"))
                {
                    if (await _galaxyMapService.SettleFaction("Azure Nebula"))
                    {
                        return Ok();
                    }
                    return BadRequest("azure failed");

                }
                return BadRequest("vega failed");

            };
            return BadRequest("solar failed");
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

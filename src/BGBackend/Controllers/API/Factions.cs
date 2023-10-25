using BrowserGameBackend.Services;
using Microsoft.AspNetCore.Mvc;
using BrowserGameBackend.Models;
using BrowserGameBackend.Services.Game;

namespace BrowserGameBackend.Controllers.API
{
    [Route("factions")]
    [ApiController]
    public class FactionsController : ControllerBase
    {
        private readonly Services.IAuthenticationService _authenticationService;
        private readonly IFactionService _factionService;

        public FactionsController(IAuthenticationService authenticationService, IFactionService factionService)
        {
            _authenticationService = authenticationService;
            _factionService = factionService;
        }

        [Route("{name}")]
        [HttpGet]
        public async Task<IActionResult> Get(string name)
        {
            Faction? faction = await _factionService.GetFaction(name)!;
            if (faction == null) return BadRequest();
            else return Ok(faction);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _factionService.GetAll());
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(string password, [FromBody] Faction factionData)
        {
            if (!_authenticationService.AdminCheck(password)) return Unauthorized();
            Faction? faction = await _factionService.Create(factionData)!;
            if(faction == null) return BadRequest("Invalid data");
            else return Ok(faction);
        }
        [HttpPut]
        public async Task<IActionResult> Update(string password, [FromBody] Faction factionData)
        {
            if (!_authenticationService.AdminCheck(password)) return Unauthorized();

            Faction? result = await _factionService.Update(factionData)!;
            if (result == null) return BadRequest("Nothing was updated");
            else return Ok(result);
        }
    }
}

using BrowserGameBackend.Models;
using BrowserGameBackend.Services.Game;
using Microsoft.AspNetCore.Mvc;
using BrowserGameBackend.Services;

namespace BrowserGameBackend.Controllers.API
{
    [Route("species")]
    [ApiController]
    public class SpeciesController : ControllerBase
    {
        private readonly Services.IAuthenticationService _authenticationService;
        private readonly ISpeciesService _speciesService;

        public SpeciesController(IAuthenticationService authenticationService, ISpeciesService speciesService)
        {
            _authenticationService = authenticationService;
            _speciesService = speciesService;
        }

        [Route("{name}")]
        [HttpGet]
        public async Task<IActionResult> Get(string name)
        {
            Species? faction = await _speciesService.GetSpecies(name)!;
            if (faction == null) return BadRequest();
            else return Ok(faction);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _speciesService.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string password, [FromBody] Species speciesData)
        {
            if (!_authenticationService.AdminCheck(password)) return Unauthorized();
            Species? species = await _speciesService.Create(speciesData)!;
            if (species == null) return BadRequest("Invalid data");
            else return Ok(species);
        }

        [HttpPut]
        public async Task<IActionResult> Update(string password, [FromBody] Species speciesData)
        {
            if (!_authenticationService.AdminCheck(password)) return Unauthorized();

            Species? result = await _speciesService.Update(speciesData)!;
            if (result == null) return BadRequest("Nothing was updated");
            else return Ok(result);
        }

    }
}

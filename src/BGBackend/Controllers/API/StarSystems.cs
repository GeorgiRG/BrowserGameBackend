using BrowserGameBackend.Dto;
using BrowserGameBackend.Models;
using BrowserGameBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BrowserGameBackend.Services.Game;
using BrowserGameBackend.Enums;

namespace BrowserGameBackend.Controllers.API
{
    [Route("starsystems")]
    [ApiController]
    public class StarSystemsController : ControllerBase
    {
        private readonly IGalaxyMapService _galaxyMapService;
        private readonly IGalaxyGenerationService _galaxyGenerationService;
        private readonly IUserManagementService _userManagementService;
        public StarSystemsController(IGalaxyMapService galaxyMapService, 
                                    IGalaxyGenerationService galaxyGenerationService, 
                                    IUserManagementService userManagementService)
        {
            _galaxyMapService = galaxyMapService;
            _galaxyGenerationService = galaxyGenerationService;
            _userManagementService = userManagementService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _galaxyMapService.GetStarSystems());
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            
            try
            {
                //UserDto? userDto = await _userManagementService.GetUserDto(sessionId: Request.Cookies["sessionId"])!;
                //if (userDto == null) return Unauthorized("Session expired");

                Console.WriteLine(id);
                List<StarSystemDto>? starDto = await _galaxyMapService.GetStarSystem(id)!;
                if (starDto != null)
                {
                    return Ok(starDto);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return NotFound();
            }
        }

        [Route("generate-initial")]
        [HttpGet]
        public async Task<IActionResult> GenerateInitial()
        {
            bool galaxyResult = await _galaxyGenerationService.GenerateGalaxy();
            bool planetsResult = await _galaxyGenerationService.GeneratePlanets();
            bool botResult = await _galaxyGenerationService.GenerateBots();
            if(!galaxyResult || !planetsResult || !botResult)
            {
                return BadRequest(
                    $"creation failed: galaxy{galaxyResult}," +
                    $"planets: {planetsResult}," + 
                    $"bots: {botResult}");
            }

            Factions factions = new();
            for(int i = 0; i < factions.Count(); i++)
            {
                if(await _galaxyGenerationService.SettleFaction(factions.FromKey(i)) == false)
                {
                    return BadRequest("settling " + factions.FromKey(i) + " failed");
                }
            }
            return Ok();
        }
    }
}

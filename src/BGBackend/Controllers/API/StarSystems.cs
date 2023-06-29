using BrowserGameBackend.Dto;
using BrowserGameBackend.Models;
using BrowserGameBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

            //await _galaxyGenerationService.GenerateGalaxy();
            //await _galaxyGenerationService.GeneratePlanets();
            //await _galaxyGenerationService.GenerateBots();
            if (await _galaxyGenerationService.SettleFaction("Solar Empire"))
            {
                if (await _galaxyGenerationService.SettleFaction("Vega Legion"))
                {
                    if (await _galaxyGenerationService.SettleFaction("Azure Nebula"))
                    {   
                        return Ok();
                    }
                    return BadRequest("azure failed");

                }
                return BadRequest("vega failed");

            };
            return BadRequest("solar failed");
        }
    }
}

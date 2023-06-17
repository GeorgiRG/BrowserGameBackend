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
        public StarSystemsController(IGalaxyMapService galaxyMapService)
        {
            _galaxyMapService = galaxyMapService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _galaxyMapService.GetStarSystems());
        }

    }

}

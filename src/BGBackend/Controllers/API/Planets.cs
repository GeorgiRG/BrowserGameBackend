using BrowserGameBackend.Services.Game;
using BrowserGameBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace BrowserGameBackend.Controllers.API
{
    [Route("planet")]
    [ApiController]
    public class PlanetsController : ControllerBase
    {
        private readonly IPlanetService _planetService;
        public PlanetsController(
            IPlanetService planetService
            )
        {
            _planetService = planetService;
        }

    }
}

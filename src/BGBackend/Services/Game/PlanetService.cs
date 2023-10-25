using BrowserGameBackend.Data;
using BrowserGameBackend.Dto;
using BrowserGameBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace BrowserGameBackend.Services.Game
{
    public interface IPlanetService
    {
        public Task<PlanetBasicViewDto>? GetPlanetBasic(int id);

        //public Task<PlanetOwnedViewDto>? GetPlanetOwned();
        //public Task<PlanetOwnedViewDto>? Settle();

    }
    public class PlanetService : IPlanetService
    {
        private readonly GameContext _context;
        public PlanetService(GameContext context)
        {
            _context = context;
        }
        public async Task<PlanetBasicViewDto>? GetPlanetBasic(int id)
        {
            PlanetBasicViewDto? result =  await _context.Planets.Where(planet => planet.Id == id)
                                        .Select(planet => new PlanetBasicViewDto(
                                            planet.Id,
                                            planet.Name,
                                            planet.Size,
                                            planet.SurfaceTemperature,
                                            planet.SystemPosition,
                                            planet.RareMetals,
                                            planet.Metals,
                                            planet.Fuels,
                                            planet.Organics,
                                            planet.Owned,
                                            planet.StarSystemId,
                                            planet.User,
                                            planet.Bot))
                                        .FirstOrDefaultAsync();
            return result!;
        }
    }
}

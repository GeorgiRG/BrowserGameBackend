using BrowserGameBackend.Data;
using BrowserGameBackend.Dto;
using BrowserGameBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Xml.Linq;

namespace BrowserGameBackend.Services.Game
{
    public interface IGalaxyMapService
    {
        public Task<Sector[]> GetSectors();
        public Task<StarSystem[]> GetSectorStarSystems(int sectorId);
        public Task<StarSystemDto>? GetStarSystem(int starId);

    }

    public class GalaxyMapService : IGalaxyMapService
    {
        private readonly GameContext _context;
        public GalaxyMapService(GameContext context)
        {
            _context = context;
        }
        public async Task<Sector[]> GetSectors()
        {
            return await _context.Sectors.OrderBy(sector => sector.Id).ToArrayAsync();
        }

        public async Task<StarSystem[]> GetSectorStarSystems(int sectorId)
        {
            return await _context.StarSystems.Where(star => star.Sector == sectorId)
                                            .OrderBy(star => star.Id)
                                            .ToArrayAsync();
        }

        public async Task<StarSystemDto>? GetStarSystem(int starId)
        {
            /*StarSystemDto? starSystemDto = 
                await _context.StarSystems.Where(star => star.Id == starId)
                                        .GroupJoin(
                                            _context.Planets,
                                            star => star,
                                            planet => planet.StarSystem,
                                            (star, planets) => new StarSystemDto
                                            (
                                            starId,
                                            star.Name!,
                                            star.LocationX,
                                            star.LocationY,
                                            star.Size,
                                            star.TotalResources,
                                            star.Faction,
                                            planets))
                                        .FirstOrDefaultAsync();*/
        StarSystemDto? starSystemDto =
            await _context.StarSystems.Where(star => star.Id == starId)
                                    .Select(
                                        star => new StarSystemDto
                                        (
                                        starId,
                                        star.Name!,
                                        star.LocationX,
                                        star.LocationY,
                                        star.Size,
                                        star.TotalResources,
                                        star.Faction,
                                        star.Planets.Select(planet => new PlanetBasicViewDto(
                                            planet.Id,
                                            planet.Name!,
                                            planet.Size,
                                            planet.SurfaceTemperature,
                                            planet.SystemPosition,
                                            planet.RareMetals,
                                            planet.Metals,
                                            planet.Fuels,
                                            planet.Organics,
                                            planet.Owned,
                                            planet.StarSystemId,
                                            planet.User!,
                                            planet.Bot
                                            ))
                                        ))
                                    .FirstOrDefaultAsync();
            return starSystemDto!;
        }

    }
}


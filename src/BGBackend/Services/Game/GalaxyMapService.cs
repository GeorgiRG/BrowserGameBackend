using BrowserGameBackend.Data;
using BrowserGameBackend.Dto;
using BrowserGameBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BrowserGameBackend.Services.Game
{
    public interface IGalaxyMapService
    {
        public Task<StarSystem[]> GetStarSystems();
        public Task<List<StarSystemDto>>? GetStarSystem(int starId);

    }

    public class GalaxyMapService : IGalaxyMapService
    {
        private readonly GameContext _context;
        public GalaxyMapService(GameContext context)
        {
            _context = context;
        }
        public async Task<StarSystem[]> GetStarSystems()
        {
            StarSystem[] starSystems = await _context.StarSystems.ToArrayAsync();
            return starSystems;
        }

        public async Task<List<StarSystemDto>>? GetStarSystem(int starId)
        {
            List<StarSystemDto>? starSystemDto = 
                await _context.StarSystems.Where(star => star.Id == starId)
                                        .GroupJoin(
                                            _context.Planets,
                                            star => star,
                                            planet => planet.StarSystem,
                                            (star, planets) => new StarSystemDto
                                            {
                                                StarSystemId = starId,
                                                Name = star.Name,
                                                LocationX = star.LocationX,
                                                LocationY = star.LocationY,
                                                TotalResources = star.TotalResources,
                                                Faction = star.Faction,
                                                Planets = planets
                                            })
                                        .ToListAsync();
            return starSystemDto!;
        }

    }
}


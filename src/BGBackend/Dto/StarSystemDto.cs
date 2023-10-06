using BrowserGameBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace BrowserGameBackend.Dto
{
    public class StarSystemDto
    {
        public int StarSystemId { get; set; }
        public string? Name { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public int Size { get; set; }
        public int TotalResources { get; set; }
        public string? Faction { get; set; }
        public IEnumerable<PlanetBasicViewDto> Planets { get; set; }
        public int FreePlanets { get; set; } = 0;

        public StarSystemDto(
            int id, string name, int locationX,
            int locationY, int size, int totalRes, string faction,
            IEnumerable<PlanetBasicViewDto> planets)
        {
            StarSystemId = id;
            Name = name;
            LocationX = locationX;
            LocationY = locationY;
            Size = size;
            TotalResources = totalRes;
            Faction = faction;
            Planets = planets;
            FreePlanets = Size - Planets.Where(planet => planet.Owned == false).Count();
        }
    }
}

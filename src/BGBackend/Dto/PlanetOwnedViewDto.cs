using BrowserGameBackend.Models;

namespace BrowserGameBackend.Dto
{
    public class PlanetOwnedViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int SurfaceTemperature { get; set; } = 0;
        public int SystemPosition { get; set; }
        public int RareMetals { get; set; } = 0;
        public int Metals { get; set; } = 0;
        public int Fuels { get; set; } = 0;
        public int Organics { get; set; } = 0;
        public int StarSystemId { get; set; }
        public int? UserId { get; set; }
        public string? OwnerName { get; set; }
        public string? OwnerFaction { get; set; }
        public Population Population { get; set; }
        public IEnumerable<ResourceProduction>? ResourceProductions { get; set; }

        public PlanetOwnedViewDto(
            int id, string name, int size, int surfTemp, int sysPos,
            int rareMet, int met, int fuels, int organics, int starSysId,
            User user, Population pop, IEnumerable<ResourceProduction>? resProd)
        {
            Id = id;
            Name = name;
            Size = size;
            SurfaceTemperature = surfTemp;
            SystemPosition = sysPos;
            RareMetals = rareMet;
            Metals = met;
            Fuels = fuels;
            Organics = organics;
            StarSystemId = starSysId;
            UserId = user.Id;
            OwnerName = user.Name;
            OwnerFaction = user.Faction;
            Population = pop;
            ResourceProductions = resProd;
                        
        }

    }
}

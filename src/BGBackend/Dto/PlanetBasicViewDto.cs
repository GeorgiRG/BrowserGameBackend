using BrowserGameBackend.Models;

namespace BrowserGameBackend.Dto
{
    public class PlanetBasicViewDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Size { get; set; }
        public int SurfaceTemperature { get; set; } = 0;
        public int SystemPosition { get; set; }
        public int RareMetals { get; set; } = 0;
        public int Metals { get; set; } = 0;
        public int Fuels { get; set; } = 0;
        public int Organics { get; set; } = 0;
        public bool Owned { get; set; }
        public int StarSystemId { get; set; }
        public int? BotId { get; set; }
        public int? UserId { get; set; }
        public string? OwnerName { get; set; }
        public string? OwnerFaction { get; set; }

        public PlanetBasicViewDto(
            int id, string name, int size, int surfTemp, int sysPos,
            int rareMet, int met, int fuels, int organics, bool owned,
            int starSysId, User? user, Bot? bot) 
        {
            Id = id;
            Name = name;
            Size = size;
            SurfaceTemperature= surfTemp;
            SystemPosition = sysPos;
            RareMetals = rareMet;
            Metals = met;
            Fuels = fuels;
            Organics = organics;
            Owned = owned;
            StarSystemId = starSysId;
            if(user != null)
            {
                UserId = user.Id;
                OwnerName = user.Name;
                OwnerFaction = user.Faction;
            }
            else if (bot != null)
            {
                BotId = bot.Id;
                OwnerName = bot.Name;
                OwnerFaction = bot.Faction;
            }

        }
    }
}

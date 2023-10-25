using BrowserGameBackend.Data;
using BrowserGameBackend.Tools;
using BrowserGameBackend.Models;
using BrowserGameBackend.Tools.GameTools;
using BrowserGameBackend.Types;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using Org.BouncyCastle.Crypto.Signers;
using BrowserGameBackend.Services.Utilities;
using System.Runtime.Serialization;
using BrowserGameBackend.Enums;
using Microsoft.Extensions.Options;
using BrowserGameBackend.Types.Options;
using BrowserGameBackend.Dto;

namespace BrowserGameBackend.Services.Game
{
    public interface IGalaxyGenerationService
    {
        public Task<bool> GenerateGalaxy();
        public Task<bool> GeneratePlanets();
        public Task<bool> GenerateBots();
        public Task<bool> SettleFaction(string faction);
        public Task<bool> CreateSectors();
    }


    /// <summary>
    /// Generates Star Systems, Planets, Bots and settles planets
    /// </summary>
    public class GalaxyGenerationService : IGalaxyGenerationService
    {
        private readonly GameContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IRandomGenerator _randomGenerator;
        private readonly GalaxyGenerationOptions _galaxyOptions;
        private readonly Factions factions = new();

        public GalaxyGenerationService(
            GameContext context,
            IDateTimeProvider dateTimeProvider,
            IRandomGenerator randomGenerator,
            IOptions<GalaxyGenerationOptions> galaxyOptions)
           
        {
            //assign actual amount from percentage of galaxy, divided by average planets of bot
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _randomGenerator = randomGenerator;
            _galaxyOptions = galaxyOptions.Value;
        }

        public async Task<bool> GenerateGalaxy()
        {
            int middleStar = (_galaxyOptions.SectorSize / 2) - (int)Math.Round(Math.Sqrt(_galaxyOptions.SectorSize) / 2) ;
            StarSystem[] stars = new StarSystem[_galaxyOptions.GalaxySize];
            int sectorHeight = (int)Math.Floor(Math.Sqrt(_galaxyOptions.SectorSize)); //10 if no changes
            for (int i = 0; i < _galaxyOptions.GalaxySize; i++)
            {
                int locationX = i % _galaxyOptions.GalaxyWidth;
                int locationY = i / _galaxyOptions.GalaxyWidth;
                int sector = locationX / sectorHeight + (locationY - (locationY % sectorHeight));
                StarSystem star = new()
                {
                    Name = _randomGenerator.IntInRange(111, 1000000).ToString("X"),
                    LocationX = locationX,
                    LocationY = locationY,
                    Sector = sector,
                    Size = _randomGenerator.IntInRange(1, 10),
                    Faction = ""
                };

                //put relays in the middle of a sector
                if((star.Sector - 1) * _galaxyOptions.SectorSize + middleStar == i)
                {
                    star.Relay = true;
                }
                stars[i] = star;
            }

            for (int i = 0; i < stars.Length; i++)
            {
                await _context.StarSystems.AddAsync(stars[i]);
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> GeneratePlanets()
        {
            StarSystem[] stars = await _context.StarSystems.OrderBy(star => star.Id).ToArrayAsync();
            List<Planet> planets = new();
            int[] capitals = GalaxyGenerationTools.CalculateCapitalSystemsId(_galaxyOptions.GalaxyWidth);
            foreach(var capital in capitals) { Console.WriteLine(capital); }
            for (int i = 0; i < stars.Length; i++)
            {
                if (!capitals.Contains(i))
                {
                    for (int j = 1; j <= stars[i].Size; j++)
                    {
                        int planetSize = _randomGenerator.IntInRange(1, _galaxyOptions.MaxPlanetSize + 1);
                        int[] resources = GalaxyGenerationTools.CalculateAvailableResources(planetSize);
                        stars[i].TotalResources += resources.Sum();
                        Planet planet = new()
                        {
                            Name = stars[i].Name + "-" + j.ToString(),
                            Size = planetSize,
                            SystemPosition = j,
                            SurfaceTemperature = _randomGenerator.IntInRange(-200, 350),
                            RareMetals = resources[0],
                            Metals = resources[1],
                            Fuels = resources[2],
                            Organics = resources[3],
                            StarSystemId = stars[i].Id,
                            StarSystem = stars[i]
                        };
                        stars[i].Planets.Add(planet);
                        planets.Add(planet);
                    }
                }
            }
            for (int i = 0; i < capitals.Length; i++)
            {
                Planet planet = new()
                {
                    Name = factions.FactionCapital(i),
                    Size = 100,
                    SystemPosition = 1,
                    SurfaceTemperature = 0,
                    StarSystemId = stars[capitals[i]].Id,
                    StarSystem = stars[capitals[i]]
                };
                stars[capitals[i]].Capital = true;
                stars[capitals[i]].Planets.Add(planet);
                stars[capitals[i]].Faction = factions.FromKey(i);
                Console.WriteLine(stars[capitals[i]].Faction + stars[capitals[i]].LocationX + " " + stars[capitals[i]].LocationY);
                planets.Add(planet);
            }
            
            for (int i = 0; i < planets.Count; i++)
            {
                await _context.Planets.AddAsync(planets[i]);
            }
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> GenerateBots()
        {
            int planetsPerSystem =  _context.Planets.Count() / _galaxyOptions.GalaxySize;
            int averageFreePlanets = (_galaxyOptions.LeaveFreeMin + _galaxyOptions.LeaveFreeMax) / 2;
            int planetsForBots = (int)(_galaxyOptions.GalaxySize * _galaxyOptions.PercentageBotControlledSystems) * planetsPerSystem;
            int averageBotPlanets = (_galaxyOptions.MaxBotPlanets + _galaxyOptions.MinBotPlanets) / 2 - averageFreePlanets;
            //bot amount is calculated based on the planets needed to control the approximate system % from settings
            int botAmount = planetsForBots / averageBotPlanets;
            Bot[] bots = new Bot[botAmount];
            FightingTraits fightingTraits = new();
            EconomyTraits economyTraits = new();

            for (int i = 0; i < bots.Length; i++)
            {
                string fightingTrait = fightingTraits.FromKey(_randomGenerator.IntInRange(0, fightingTraits.Count()));
                string economyTrait = economyTraits.FromKey(_randomGenerator.IntInRange(0, economyTraits.Count()));
                string faction = factions.FromKey(_randomGenerator.IntInRange(0, factions.Count()));
                int age = _randomGenerator.IntInRange(5, 91);
                bots[i] = new Bot()
                {
                    Name = "bot for now",
                    FightingTrait = fightingTrait,
                    EconomyTrait = economyTrait,
                    Age = age,
                    PowerLevel = 0, //calculated later
                    AvailableResources = 0,
                    PlanetsAmount = _randomGenerator.IntInRange(_galaxyOptions.MinBotPlanets, _galaxyOptions.MaxBotPlanets),
                    Faction = faction,
                    FleetTemplate = _randomGenerator.IntInRange(1, _galaxyOptions.FleetTemplateDifficulty),
                    LastChecked = _dateTimeProvider.UtcNow()
                };
            }
            for (int i = 0; i < factions.Count(); i++)
            {
                string faction = factions.FromKey(i);
                await _context.Bots.AddAsync(GalaxyGenerationTools.GenerateRuler(faction));
            }
            for (int i = 0; i < bots.Length; i++)
            {
                await _context.Bots.AddAsync(bots[i]);
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SettleFaction(string faction)
        {
            StarSystem capital = 
                await _context.StarSystems.Where(system => system.Faction == faction)
                                        .FirstAsync();
            // ruler has the highest power lvl
            Bot? rulerBot = await _context.Bots.Where(bot => bot.Faction == faction)
                                            .OrderByDescending(bot => bot.PowerLevel)
                                            .FirstAsync();
            Planet[] planets =
                await _context.Planets.Where(planet => planet.StarSystemId == capital.Id && planet.Owned == false)
                                    .ToArrayAsync();
            for (int j = 0; j < planets.Length; j++)
            {
                planets[j].BotId = rulerBot.Id;
                planets[j].Bot = rulerBot;
                rulerBot.PlanetsAmount--;
            }
            bool result = await _context.SaveChangesAsync() > 0;
            bool botsResult = await SettleBots(capital.LocationX, capital.LocationY, faction);
            if (result && botsResult)
            {
                return true;
            }
            else
            {
                Console.WriteLine($"rulers settled - {result}, bots settled - {botsResult}");
                return false;
            }
        }

        //settles all bots
        public async Task<bool> SettleBots(int capitalX, int capitalY, string faction)
        {
            Coordinates coor = new ();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                int radius = 1;
                Bot[] bots =
                    await _context.Bots.Where(bot => bot.Faction == faction && bot.PlanetsAmount > 0)
                                    .ToArrayAsync();
                //find all planets to settle
                int planetsLeftToSettle = 0;
                for (int i = 0; i < bots.Length; i++)
                {
                    planetsLeftToSettle += bots[i].PlanetsAmount;
                }
                Console.WriteLine(planetsLeftToSettle);
                int currentBotPos = 0;
                while (planetsLeftToSettle > 0)
                {
                    List<Coordinates> locations = GalaxyMovementTool.GetStarSystemsInRadius(capitalX, capitalY, radius, _galaxyOptions.GalaxyWidth);
                    for (int i = 0; i < locations.Count; i++)
                    {
                        if (planetsLeftToSettle == 0)
                        {
                            break;
                        }

                        if (locations[i] != null)
                        {
                            StarSystem? star = 
                                await _context.StarSystems.Where(star => star.LocationX == locations[i].X
                                                                       && star.LocationY == locations[i].Y)
                                                        .FirstAsync();
                            star.Faction = faction; //make sure beforehand the amount of bots is such that they don't leak in factions' systems

                            Planet[] planets = 
                                await _context.Planets.Where(planet => planet.StarSystemId == star.Id && planet.Owned == false)
                                                     .ToArrayAsync();
                            int current = 0;
                            while (current < planets.Length && planetsLeftToSettle > 0)
                            {
                                if (currentBotPos > bots.Length - 1)
                                {
                                    currentBotPos = 0; //goes over bot list multiple times
                                }
                                if (bots[currentBotPos].PlanetsAmount > 0)
                                {
                                    planets[current].BotId = bots[currentBotPos].Id;
                                    planets[current].Bot = bots[currentBotPos];
                                    planets[current].Owned = true;
                                    bots[currentBotPos].Planets.Add(planets[current].Id);
                                    bots[currentBotPos].AvailableResources +=
                                                            planets[current].RareMetals + planets[current].Metals
                                                            + planets[current].Fuels + planets[current].Organics;
                                    bots[currentBotPos].PlanetsAmount--;
                                    planetsLeftToSettle--;
                                    current +=
                                        //skip planets to have slots for players
                                        _randomGenerator.IntInRange(_galaxyOptions.LeaveFreeMin, _galaxyOptions.LeaveFreeMax); 
                                    currentBotPos++;
                                }
                                else
                                {
                                    currentBotPos++;
                                }
                            }
                        }
                    }
                    radius++;
                }
                await _context.SaveChangesAsync();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "\n" + "Settling bots failed");
                transaction.Rollback();
                return false;
            }
        }

        public async Task<bool> CreateSectors()
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                int sectorAmount = _galaxyOptions.GalaxySize / _galaxyOptions.SectorSize;
                for (int i = 1; i <= sectorAmount; i++)
                {
                    HashSet<int> playerIds = new();
                    int totalPlanets = 0;
                    List<StarSystemDto> starSystems =
                        await _context.StarSystems.Where(star => star.Sector == i).Select(
                                        star => new StarSystemDto
                                        (
                                        star.Id,
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
                                            planet.User,
                                            planet.Bot
                                            ))
                                        ))
                                        .ToListAsync();
                    int bots = 0;
                    int players = 0;
                    for (int j = 0; j < starSystems.Count; j++)
                    {
                        totalPlanets += starSystems[j].Planets.Count();

                        foreach (PlanetBasicViewDto planet in starSystems[j].Planets)
                        {
                            if (planet.BotId != null)
                            {
                                bots++;
                            }
                            else if(planet.UserId != null)
                            {
                                players++;
                            }
                        }
                    }

                    Sector sector = new()
                    {
                        GameId = i,
                        TotalPlanets = totalPlanets,
                        Players = players,
                        Bots = bots,
                        FreeSystems = starSystems.Where(star => star.Faction == String.Empty).Count(),
                        VegaControl = starSystems.Where(star => star.Faction == factions.Vega).Count() * 100 / _galaxyOptions.SectorSize,
                        AzureControl = starSystems.Where(star => star.Faction == factions.Azure).Count() * 100 / _galaxyOptions.SectorSize,
                        SolarControl = starSystems.Where(star => star.Faction == factions.Solar).Count() * 100 / _galaxyOptions.SectorSize,
                        RobotsControl = starSystems.Where(star => star.Faction == factions.NaturalOrder).Count() * 100 / _galaxyOptions.SectorSize,
                        SwarmControl = starSystems.Where(star => star.Faction == factions.Swarm).Count() * 100 / _galaxyOptions.SectorSize,
                        PandemoniumControl = starSystems.Where(star => star.Faction == factions.Pandemonium).Count() * 100 / _galaxyOptions.SectorSize
                    };
                    await _context.Sectors.AddAsync(sector);
                }
                await _context.SaveChangesAsync();
                transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "\n" + "Sector creation failed");
                transaction.Rollback();
                return false;
            }      
        }
    }
}

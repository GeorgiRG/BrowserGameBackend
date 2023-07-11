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

namespace BrowserGameBackend.Services.Game
{
    public interface IGalaxyGenerationService
    {
        public Task<bool> GenerateGalaxy();
        public Task<bool> GeneratePlanets();
        public Task<bool> GenerateBots();
        public Task<bool> SettleFaction(string faction);
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
       
            StarSystem[] stars = new StarSystem[_galaxyOptions.GalaxySize];
            for (int i = 0; i < _galaxyOptions.GalaxySize; i++)
            {
                StarSystem star = new()
                {
                    Name = _randomGenerator.IntInRange(111, 99999).ToString("X"),
                    LocationX = i % _galaxyOptions.GalaxyWidth,
                    LocationY = i / _galaxyOptions.GalaxyWidth,
                    Sector = i / _galaxyOptions.SectorSize,
                    Size = _randomGenerator.IntInRange(1, 9),
                    Faction = ""
                };
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
            StarSystem[] stars = await _context.StarSystems.ToArrayAsync();
            List<Planet> planets = new();
            int[] capitals = GalaxyGenerationTools.CalculateCapitalSystemsId(_galaxyOptions.GalaxyWidth);
            Factions factions = new();
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
                stars[capitals[i]].Planets.Add(planet);
                stars[capitals[i]].Faction = factions.FromKey(i);
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
            Factions factions = new();

            for (int i = 0; i < bots.Length; i++)
            {
                string fightingTrait = fightingTraits.FromKey(_randomGenerator.IntInRange(0, fightingTraits.Count() - 1));
                string economyTrait = fightingTraits.FromKey(_randomGenerator.IntInRange(0, economyTraits.Count() - 1));
                string faction = factions.FromKey(_randomGenerator.IntInRange(0, factions.Count() - 1));
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
                await _context.Planets.Where(planet => planet.StarSystemId == capital.Id && planet.OwnerId == 0)
                                    .ToArrayAsync();
            for (int j = 0; j < planets.Length; j++)
            {
                planets[j].OwnerId = rulerBot.Id;
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
                                await _context.Planets.Where(planet => planet.StarSystemId == star.Id && planet.OwnerId == 0)
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
                                    planets[current].OwnerId = bots[currentBotPos].Id;
                                    bots[currentBotPos].AvailableResources =
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
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                transaction.Rollback();
                return false;
            }
        }
    }
}

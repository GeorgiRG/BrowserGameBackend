using BrowserGameBackend.Data;
using BrowserGameBackend.Tools;
using BrowserGameBackend.Models;
using BrowserGameBackend.Tools.GameTools;
using BrowserGameBackend.Types;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using Org.BouncyCastle.Crypto.Signers;

namespace BrowserGameBackend.Services
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
        public GalaxyGenerationService(GameContext context)
        {
            _context = context;
        }

        public async Task<bool> GenerateGalaxy()
        {
            StarSystem[] stars = GeneratorTool.GenerateSystems();
            for (int i = 0; i < stars.Length; i++)
            {
                await _context.StarSystems.AddAsync(stars[i]);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GeneratePlanets()
        {
            StarSystem[] stars = await _context.StarSystems.ToArrayAsync();
            List<Planet> planets = GeneratorTool.GeneratePlanets(ref stars);
            for (int i = 0; i < planets.Count; i++)
            {
                await _context.Planets.AddAsync(planets[i]);
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> GenerateBots()
        {
            Bot[] rulers = GeneratorTool.GenerateRulers();
            for (int i = 0; i < rulers.Length; i++)
            {
                await _context.Bots.AddAsync(rulers[i]);
            }
            Bot[] bots = GeneratorTool.GenerateBots();
            for (int i = 0; i < bots.Length; i++)
            {
                await _context.Bots.AddAsync(bots[i]);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SettleFaction(string faction)
        {
            StarSystem capital = await _context.StarSystems
                                            .Where(system => system.Size == GeneratorTool.CapitalSystemSize && system.Faction == faction)
                                            .FirstAsync();
            //settle capital systems by the ruler

            Bot? rulerBot = await _context.Bots
                                    .Where(bot => bot.Faction == faction)
                                    .OrderByDescending(bot => bot.PowerLevel)
                                    .FirstAsync();
            Planet[] planets = await _context.Planets
                                        .Where(planet => planet.StarSystemId == capital.Id && planet.OwnerId == 0)
                                        .ToArrayAsync();
            for (int j = 0; j < planets.Length; j++)
            {
                planets[j].OwnerId = rulerBot.Id;
                rulerBot.PlanetsAmount--;
            }
            //saves changes
            if (await SettleBots(capital.LocationX, capital.LocationY, faction)) 
            {
                return await _context.SaveChangesAsync() > 0;
            }
            else return false;
        }

        //settles all bots
        //leaves {radius} empty planets on each loop so there is more concentration around capital
        //all in one function as it actually makes it simpler
        public async Task<bool> SettleBots(int capitalX, int capitalY, string faction)
        {
            Coordinates coor = new Coordinates();
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Random random = new Random();
                int radius = 1;
                Bot[] bots = await _context.Bots
                                            .Where(bot => bot.Faction == faction && bot.PlanetsAmount > 0)
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
                    List<Coordinates> locations = GalaxyMovementTool.GetStarSystemsInRadius(capitalX, capitalY, radius);
                    for (int i = 0; i < locations.Count; i++)
                    {
                        if (planetsLeftToSettle == 0)
                        {
                            break;
                        }

                        if (locations[i] != null)
                        {
                            StarSystem? star = await _context.StarSystems
                                                                .Where(star => star.LocationX == locations[i].X
                                                                       && star.LocationY == locations[i].Y)
                                                                .FirstAsync();
                            star.Faction = faction; //make sure beforehand the amount of bots is such that they don't leak in factions' systems

                            Planet[] planets = await _context.Planets
                                                                .Where(planet => planet.StarSystemId == star.Id && planet.OwnerId == 0)
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
                                    current += random.Next(1, 3); //skip planets to have slots for players
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
                    Console.WriteLine(radius + " radius");
                }
                transaction.Commit();
                return true;
                //return await _context.SaveChangesAsync() > 0;
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

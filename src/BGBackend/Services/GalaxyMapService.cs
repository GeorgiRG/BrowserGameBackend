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
    public interface IGalaxyMapService
    {
        public Task<StarSystem[]> GetStarSystems();
        public Task<bool> GenerateGalaxy();
        public Task<bool> GeneratePlanets();
        public Task<bool> GenerateBots();
        public Task<bool> SettleFaction(string faction);

    }
    /// <summary>
    /// Generates Star Systems, Planets, Bots and settles planets
    /// </summary>
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

        public async Task<bool> GenerateGalaxy()
        {
            StarSystem[] stars = GeneratorTool.GenerateSystems();
            for(int i = 0; i < stars.Length; i++)
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
                                        .Where(planet => planet.SystemId == capital.Id && planet.OwnerId == 0)
                                        .ToArrayAsync();
            for(int j = 0; j < planets.Length; j++)
            {
                planets[j].OwnerId = rulerBot.Id;
                rulerBot.PlanetsAmount--;
            }
            //saves changes
            return await SettleBots(capital.LocationX, capital.LocationY, faction);
        }
        //settles all bots
        //leaves {radius} empty planets on each loop so there is more concentration around capital
        //all in one function as it actually makes it simpler
        public async Task<bool> SettleBots(int capitalX, int capitalY, string faction)
        {
            using var transaction = _context.Database.BeginTransaction();
            try {
                int radius = 1;
                Bot[] bots = await _context.Bots
                                            .Where(bot => bot.Faction == faction && bot.PlanetsAmount > 0)
                                            .ToArrayAsync();
                //find all planets to settle
                int planetsLeftToSettle = 0;
                for(int i = 0; i < bots.Length; i++)
                {
                    planetsLeftToSettle += bots[i].PlanetsAmount;
                }
                int currentBotPos = 0;
                while (planetsLeftToSettle > 0)
                {
                    //8 locations, can be null, second array is [X, Y]
                    int[][] locations = GalaxyMovementTool.GetStarSystemsInRadius(capitalX, capitalY, radius);
                    for(int i = 0; i < locations.Length; i++)
                    {
                        if(planetsLeftToSettle == 0)
                        {
                            break;
                        }

                        if (locations[i] != null)
                        {
                            StarSystem? star = await _context.StarSystems
                                                    .Where(star => star.LocationX == locations[i][0]
                                                           && star.LocationY == locations[i][1])
                                                    .FirstAsync();
                            star.Faction = faction; //make sure beforehand the amount of bots is such that they don't leak in factions' systems
                            //to avoid getting stuck in the loop below
                            int reducePlanets = 0;
                            if (star.Size - radius > planetsLeftToSettle)
                            {
                                reducePlanets = star.Size - radius - planetsLeftToSettle;
                            }

                            Planet[] planets = await _context.Planets
                                                        .Where(planet => planet.SystemId == star.Id && planet.OwnerId == 0)
                                                        .ToArrayAsync();
                            for(int j = 0; j < planets.Length - radius - reducePlanets; j++)
                            {
                                int botCount = 0;
                                while (bots[currentBotPos].PlanetsAmount == 0 && botCount < bots.Length)
                                {
                                    currentBotPos++;
                                    botCount++; //break if it loops over all bots

                                    if(currentBotPos >= bots.Length)
                                    {
                                        currentBotPos = 0;
                                    }
                                }

                                planets[j].OwnerId = bots[currentBotPos].Id;
                                bots[currentBotPos].AvailableResources = 
                                                        planets[j].RareMetals + planets[j].Metals
                                                        + planets[j].Fuels + planets[j].Organics;
                                bots[currentBotPos].PlanetsAmount--;
                                planetsLeftToSettle--;
                                currentBotPos++;
                                if (currentBotPos >= bots.Length)
                                {
                                    currentBotPos = 0;
                                }
                            }
                        }
                    }
                    radius++;

                }

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception) { 
                transaction.Rollback();
                return false;
            }
        }
    }
}

using BrowserGameBackend.Models;
using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace BrowserGameBackend.Tools.GameTools
{
    public static class GeneratorTool
    {
        public const int galaxySize = 100;
        public const int galaxyWidth = 10;
        public const int CapitalSystemSize = 11;
        public static StarSystem[] GenerateSystems()
        {
            Random random = new();
            StarSystem[] stars = new StarSystem[galaxySize];
            for (int i = 0; i < galaxySize; i++)
            {
                StarSystem star = new()
                {
                    Name = random.Next(111, 99999).ToString("X"),

                    LocationX = i % galaxyWidth,
                    LocationY = i / galaxyWidth,
                    Size = random.Next(1, 9),
                    Faction = ""
                };
                stars[i] = star;
            }
            return stars;

        }
        public static List<Planet> GeneratePlanets(ref StarSystem[] stars)
        {
            Random random = new();
            List<Planet> planets = new ();
            int[] capitals = DecideCapitalSystems();
            for (int i = 0; i < stars.Length; i++)
            {
                if (capitals.Contains(i))
                {
                    stars[i].Size = CapitalSystemSize;
                    if (i == capitals[0])
                    {
                        stars[i].Faction = "Solar Empire";
                    }
                    else if (i == capitals[1])
                    {
                        stars[i].Faction = "Vega Legion";
                    }
                    else
                    {
                        stars[i].Faction = "Azure Nebula";
                    }
                }
                for (int j = 1; j <= stars[i].Size; j++)
                {
                    int planetSize = random.Next(1, 51);
                    if (j == CapitalSystemSize) planetSize = 70;
                    int[] resources = DecideResources(planetSize);
                        stars[i].TotalResources += resources.Sum();
                        Planet planet = new()
                        {
                            SystemId = stars[i].Id,
                            Name = stars[i].Name + "-" + j.ToString(),
                            Size = planetSize,
                            Type = DecidePlanetType(planetSize),
                            SystemPosition = j,
                            RareMetals = resources[0],
                            Metals = resources[1],
                            Fuels = resources[2],
                            Organics = resources[3],
                        };
                        planets.Add(planet);
                    
                }
            }
            return planets;

        }
        public static int[] DecideCapitalSystems()
        {
            int left = (int)(0.3 * galaxyWidth + 7 * galaxyWidth);
            int right = (int)(0.7 * galaxyWidth + 7 * galaxyWidth);
            int bottom = (int)(0.5 * galaxyWidth + 4 * galaxyWidth);
            //coordinates will be (3, 7) (7, 7) (5, 4) at width 10 for their respective Ids
            return new int[] { left, right, bottom };
        }

        public static Planet GenerateCapitalSystemPlanets(StarSystem capital)
        {
            Planet planet = new ()
            {
                SystemId = capital.Id,
                Name = capital.Name + "base",
                Size = 50,
                Type = DecidePlanetType(50),
                SystemPosition = 3,
                RareMetals = 50,
                Metals = 50,
                Fuels = 50,
                Organics = 50
            };
            return planet;
            
            /*
            capitals[0].Name = "Solar System";
            capitals[0].Faction = "Solar Empire";
            planets[0].Name = "Nova Terra";
            capitals[1].Name = "Vega";
            capitals[1].Faction = "Vega Legion";
            planets[1].Name = "Vega Prime";
            capitals[2].Name = "Funny megacorp reference";
            capitals[2].Faction = "Azure Nebula";
            planets[2].Name = "Another funny reference";
            return planets;*/
        }

        public static Bot[] GenerateRulers()
        {
            Bot[] rulers = new Bot[3];
            rulers[0] = new Bot()
            {
                Name = "Red Empress",
                FightingTrait = "Ruthless",
                EconomyTrait = "Conqueror",
                PowerLevel = 999,
                AvailableResources = 999,
                Age = 1000,
                PlanetsAmount = CapitalSystemSize,
                Faction = "Solar Empire",
                FleetTemplate = 100,
                LastChecked = DateTime.UtcNow
            };
            rulers[1] = new Bot()
            {
                Name = "The Prophet of Vega",
                FightingTrait = "Fanatic",
                EconomyTrait = "Saviour",
                PowerLevel = 999,
                AvailableResources = 999,
                Age = 1000,
                PlanetsAmount = CapitalSystemSize,
                Faction = "Vega Legion",
                FleetTemplate = 200,
                LastChecked = DateTime.UtcNow
            };
            rulers[2] = new Bot()
            {
                Name = "The CEO",
                FightingTrait = "Shrewd",
                EconomyTrait = "Degenerate",
                PowerLevel = 999,
                AvailableResources = 999,
                Age = 1000,
                PlanetsAmount = CapitalSystemSize,
                Faction = "Azure Nebula",
                FleetTemplate = 300,
                LastChecked = DateTime.UtcNow
            };
            return rulers;
        }
        public static Bot[] GenerateBots()
        {
            Bot[] bots = new Bot[galaxyWidth * 3];
            Random random= new ();
            for(int i = 0; i < bots.Length; i++) 
            {
                string[] traits = DecideBotTraits();
                int age = random.Next (5, 91);
                bots[i] = new Bot()
                {
                    Name = "bot for now",
                    FightingTrait = traits[0],
                    EconomyTrait = traits[1],
                    Age = age,
                    PowerLevel = 0, //calculated later
                    AvailableResources = 0,
                    PlanetsAmount = random.Next(3,9),
                    Faction = DecideFaction(random.Next(4)),
                    FleetTemplate = random.Next(6),
                    LastChecked = DateTime.UtcNow
                };
            }
            return bots;
        }

        public static string DecideFaction(int roll)
        {
            //Add Anarchists, Robots and Parasites
            return roll switch
            {
                0 => "Solar Empire",
                1 => "Vega Legion",
                2 => "Azure Nebula",
                3 => "Unaffiliated",
                _ => "",
            };
        }

        public static string DecidePlanetType(int size)
        {
            if(size <= 10)
            {
                return "Dwarf Planet";
            }
            else if (size <= 25) 
            {
                return "Terrestrial";
            }
            else
            {
                return "Giga Terrestrial";
            }
        }

        public static int[] DecideResources(int planetSize)
        {
            //Rare Metals, Metals, Fuels, Organics
            int[] resources = new int[4];

            float total = 0;
            Random random = new ();
            int totalAllowed = random.Next(4 + planetSize/3, 4 + (int)(planetSize * 1.5));
            for(int i = 0; i < 4; i++) 
            {
                //decide ratios
                resources[i] = random.Next(1, 10);
                total += resources[i];
            }
            for (int i = 0; i < 4; i++)
            {
                //assign actual res
                resources[i] = (int)Math.Round((resources[i] / total) * totalAllowed);
            }
            return resources;
        }

        public static string[] DecideBotTraits() 
        {
            string[] traits = new string[2];
            Random random = new();
            int fighting = random.Next(6);
            int economy = random.Next(6);
            traits[0] = fighting switch
            {
                0 => "Glory-seeking",
                1 => "Peaceful",
                2 => "Relentless",
                3 => "Careful",
                4 => "Vengeful",
                5 => "Opportunistic",
                _ => "Peaceful",
            };
            traits[1] = economy switch
            {
                0 => "Researcher",
                1 => "Expansionist",
                2 => "Trader",
                3 => "Manufacturer",
                4 => "Fabricator",
                5 => "Commander",
                _ => "Commander",
            };
            return traits;
        }
    }
}
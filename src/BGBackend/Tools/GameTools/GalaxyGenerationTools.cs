using BrowserGameBackend.Enums;
using BrowserGameBackend.Models;
using System;
using System.Net.NetworkInformation;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace BrowserGameBackend.Tools.GameTools
{
    public static class GalaxyGenerationTools
    {
        //gSize must be square of gWidth for capital X,Y to work
        public const int CapitalSystemSize = 11;
        public const int maxPlanetSize = 51;
        public const int minResources = 5;

        public static int[] CalculateCapitalSystemsId(int galaxyWidth)
        {
            //calculate capital Ids based on their XY
            //at gWidth 100 -> 0.5 * 100 + 0.5 * 10000 = 5500 ID
            //they are spread at edges like a hexagon
            //locations are predetermined to have minor factions be based
            //around their origin faction
            int[] locations = new int[6] //6 factions, ordered as the Factions enum, move it to factions enum
            {
                (int)(0.5 * galaxyWidth + 0 * galaxyWidth * galaxyWidth),   //robots - X 50%:Y 0%
                (int)(0.9 * galaxyWidth + 0.3 * galaxyWidth * galaxyWidth), //vega- X 90%:Y 30%
                (int)(0.9 * galaxyWidth + 0.7 * galaxyWidth * galaxyWidth), //parasites- X 90%:Y 70%
                (int)(0.5 * galaxyWidth + 0.9 * galaxyWidth * galaxyWidth), //solar - X 50%:Y 90%
                (int)(0 * galaxyWidth + 0.7 * galaxyWidth * galaxyWidth),   //anarchists - X 0%:Y 70%
                (int)(0 * galaxyWidth + 0.4 * galaxyWidth * galaxyWidth),   //azure - X 0%:Y 30%
            };
            return locations;
        }

        public static Bot GenerateRuler(string faction)
        {
            Console.WriteLine(faction + " faction ");
            Bot ruler = new()
            {
                PowerLevel = 999,
                AvailableResources = 999,
                Age = 1000,
                PlanetsAmount = 1,
                Faction = faction,
                LastChecked = DateTime.UtcNow
            };
            switch (faction)
            {
                case "Solar Empire":
                    ruler.FleetTemplate = 100;
                    ruler.Name = "The Red Empress";
                    ruler.FightingTrait = "Ruthless";
                    ruler.EconomyTrait = "Conqueror";
                    break;
                case "Vega Legion":
                    ruler.Name = "The Prophet of Vega";
                    ruler.FleetTemplate = 200;
                    ruler.FightingTrait = "Fanatic";
                    ruler.EconomyTrait = "Saviour";
                    break;
                case "Azure Nebula":
                    ruler.Name = "The CEO";
                    ruler.FleetTemplate = 300;
                    ruler.FightingTrait = "Shrewd";
                    ruler.EconomyTrait = "Degenerate";
                    break;
                case "Natural Order":
                    ruler.Name = "The Big Iron";
                    ruler.FleetTemplate = 400;
                    ruler.FightingTrait = "Flawless";
                    ruler.EconomyTrait = "Purifier";
                    break;
                case "Swarm":
                    ruler.Name = "The Pestilence";
                    ruler.FleetTemplate = 500;
                    ruler.FightingTrait = "Mindless";
                    ruler.EconomyTrait = "Devourer";
                    break;
                case "Pandemonium":
                    ruler.Name = "The Grey Council";
                    ruler.FleetTemplate = 600;
                    ruler.FightingTrait = "Chaotic";
                    ruler.EconomyTrait = "Liberator";
                    break;
                default:
                    ruler.Name = "failed";
                    ruler.FleetTemplate = 500;
                    ruler.FightingTrait = "failed";
                    ruler.EconomyTrait = "failed";
                    break;
            }
            return ruler;
        }

        public static int[] CalculateAvailableResources(int planetSize)
        {
            //Rare Metals, Metals, Fuels, Organics
            int[] resources = new int[4];

            float total = 0;
            Random random = new ();
            //1,5 to buff larger planets
            int totalAllowed = random.Next(minResources + planetSize, minResources + (int)(planetSize * 1.5));
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
    }
}
using System.Security.Cryptography.X509Certificates;

namespace BrowserGameBackend.Models
{
    public class Population
    {
        public int Id { get; set; }
        public int Total { get; set; }
        public int Loyalty { get; set; }
        public int Aquatics { get; set; }
        public int Humans { get; set; }
        public int Insects { get; set; }
        public int Liths { get; set; }
        public int Robots { get; set; }
        public int ResearchTeams { get; set; }
        public int ProductionTeams { get; set; }
        public int FinancesTeams { get; set; }
        public int MillitaryTeams { get; set; }
        //used in PopulationTeamsTool, to calculate ratio of returned pops on team disband
        public int AquaticInTeams { get; set; } = 0;
        public int HumansInTeams { get; set; } = 0;
        public int InsectsInTeams { get; set; } = 0; 
        public int LithsInTeams { get; set; } = 0;
        public int RobotsInTeams { get; set; } = 0;

        public int PlanetId { get; set; }
        public Planet Planet { get; set; } = null!;
    }
}

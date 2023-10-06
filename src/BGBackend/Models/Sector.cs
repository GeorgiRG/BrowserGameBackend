namespace BrowserGameBackend.Models
{
    /// <summary>
    /// Holds a summary of the star systems in a sector that shows on hovering
    /// Updated through quartz jobs 
    /// 'Control' shows what % of the sector each faction has
    /// 
    /// Not connected to star systems in db as there might be a rework
    /// </summary>
    public class Sector
    {
        public int Id { get; set; }
        public int GameId { get; set; } = 0;
        public int Players { get; set; } = 0;
        public int Bots { get; set; } = 0;
        public int TotalPlanets { get; set; }
        public int FreeSystems { get; set; }
        public int SolarControl { get; set; } = 0;
        public int VegaControl { get; set; } = 0;
        public int AzureControl { get; set; } = 0;   
        public int RobotsControl { get; set; } = 0;
        public int PandemoniumControl { get; set; } = 0;
        public int SwarmControl { get; set; } = 0;

    }
}

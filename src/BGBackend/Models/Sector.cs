namespace BrowserGameBackend.Models
{
    /// <summary>
    /// Holds a summary of the star systems in a sector that shows on hovering
    /// Updated through quartz jobs 
    /// 'Control' shows what % of the sector each faction has
    /// </summary>
    public class Sector
    {
        public int Id { get; set; }
        public int GameId { get; set; } = 0;
        public int Players { get; set; } = 0;
        public int Bots { get; set; } = 0;
        public int TotalPlanets { get; set; }
        public int FreeSystems { get; set; }
        public int RedControl { get; set; } = 0;
        public int GreenControl { get; set; } = 0;
        public int BlueControl { get; set; } = 0;   
        public int GreyControl { get; set; } = 0; 
    }
}

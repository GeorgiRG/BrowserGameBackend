namespace BrowserGameBackend.Types.Options
{
    public class GalaxyGenerationOptions
    {
        public const string GalaxyOptions = "GalaxyOptions";

        public int GalaxySize { get; set; }
        public int GalaxyWidth { get; set; }
        public int SectorSize { get; set; }
        public int MinBotPlanets { get; set; }
        public int MaxBotPlanets { get; set; }
        public int MaxPlanetSize { get; set; }
        public int MinPlanetResources { get; set; }
        public int FleetTemplateDifficulty { get; set; }//Higher fleet numbers have better composition, 99 is max
        public float PercentageBotControlledSystems { get; set; }
        //if faction controlled systems should have free planets 
        public int LeaveFreeMin { get; set; } 
        public int LeaveFreeMax { get; set; }
    }
}
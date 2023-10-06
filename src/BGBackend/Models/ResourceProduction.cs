namespace BrowserGameBackend.Models
{
    //used for planets
    public class ResourceProduction
    {
        public int Id { get; set; }
        public ResourceStorage? ResourceStorage { get; set; } = null!;
        public int Efficiency { get; set; } = 0; //for application of bonuses or smth, gov skills?
        public int Factories { get; set; } = 0;
        public int WorkerTeams { get; set; } = 0;
        public int ResourceId { get; set; }
        public Resource Resource { get; set; } = null!;
        public int PlanetId { get; set; }
        public Planet Planet { get; set; } = null!;
    }
}
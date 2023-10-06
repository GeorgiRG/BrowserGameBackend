namespace BrowserGameBackend.Models
{
    public class ResourceStorage
    {
        public int Id { get; set; }
        public int Amount { get; set; } = 0;
        public int ResourceId { get; set; }
        public Resource Resource { get; set; } = null!;
        public int? FleetID { get; set; }
        public Fleet? Fleet { get; set; }
        public int? ResourceProductionId { get; set; }
        public ResourceProduction? ResourceProduction { get; set; }
    }
}

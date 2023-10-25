namespace BrowserGameBackend.Models
{
    public class Species
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public float? ReproductionSpeed { get; set; }
        public float? ResearchSpeed { get; set; }
        public float? ProductionSpeed { get; set; }
        public float? CombatStrength { get; set; }
        public float? SuitComplexity { get; set; }
        public int? TemperatureRange { get; set; } //if 10 and average is 20, comfortable range is 10-30
        public int? TemperatureAverage { get; set; }
        public int? Loyalty { get; set; }
    }
}

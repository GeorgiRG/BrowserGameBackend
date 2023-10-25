namespace BrowserGameBackend.Models
{
    public class Faction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
        public int CapitalSystemId { get; set; } = 0;
        public int RulerId { get; set; }
        public Bot? Ruler { get; set; }
        public int? SomeModifier { get; set; } = 0;
        //buildings, units, ships, modifiers, etc.
        //stuff
    }
}

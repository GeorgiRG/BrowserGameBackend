using System.ComponentModel.DataAnnotations;

namespace BrowserGameBackend.Models
{
    public class StarSystem
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public int Sector { get; set; }
        public int Size { get; set; }
        public bool Contested { get; set; }
        public int TotalResources { get; set; }
        public string Faction { get; set; } = string.Empty;
        public bool Relay { get; set; } = false;
        public ICollection<Planet> Planets { get; } = new List<Planet>();
    }
}

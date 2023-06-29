using BrowserGameBackend.Models;
using System.ComponentModel.DataAnnotations;

namespace BrowserGameBackend.Dto
{
    public class StarSystemDto
    {
        public int StarSystemId { get; set; }
        public string? Name { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public int Size { get; set; }
        public int TotalResources { get; set; }
        public string? Faction { get; set; }
        public IEnumerable<Planet> Planets { get; set; }
    }
}

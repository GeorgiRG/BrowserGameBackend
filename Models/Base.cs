using System.ComponentModel.DataAnnotations;

namespace BrowserGameBackend.Models
{
    public class Base
    {
        public int BaseID { get; set; }
        public string? Name { get; set; }
        public int Size { get; set; }
        public string? Location { get; set; }
        public string? Faction { get; set; }
        public string? Description { get; set; }
        [Required]
        public User? Owner { get; set; }

    }
}

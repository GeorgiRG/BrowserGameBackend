using System.ComponentModel.DataAnnotations;

namespace BrowserGameBackend.Models
{
    public class Colony
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Size { get; set; }
        public string? Location { get; set; }
        public string? Faction { get; set; }
        public string? Description { get; set; }
        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }

    }
}

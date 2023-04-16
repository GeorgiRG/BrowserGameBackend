using System.ComponentModel.DataAnnotations;

namespace BrowserGameBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? Faction { get; set; }
        public string? Race { get; set; }
        public string? Class { get; set; }
        public string EmailConfirmation { get; set; } = "No";
    }
}

using System.ComponentModel.DataAnnotations;

namespace BrowserGameBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? Faction { get; set; }
        public string? Race { get; set; }
        public string? CharClass { get; set; }
        public string EmailConfirmation { get; set; } = "No";
        public string? SessionId { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}

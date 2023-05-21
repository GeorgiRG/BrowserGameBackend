using Microsoft.Extensions.Hosting;
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
        public string? Species { get; set; }
        public string EmailConfirmation { get; set; } = "No";
        public string SessionId { get; set; } = "1";
        public DateTime? LastLogin { get; set; }
        public ICollection<Colony> Colonies { get; set; } = new List<Colony>();
        public int UserSkillsId { get; set; }
        public UserSkills? UserSkills { get; set;  }


    }
}

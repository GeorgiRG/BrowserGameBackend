using System.ComponentModel.DataAnnotations;

namespace BrowserGameBackend.Models
{
    public class UserSkills
    {
        public int Id { get; set; }
        public int Level { get; set; } = 0;
        public int Experience { get; set; } = 0;
        public int SpaceWarfare { get; set; } = 5;
        public int LandWarfare { get; set; } = 5;
        public int Research { get; set; } = 5;
        public int Engineering { get; set; } = 5;
        public int Economy { get; set; } = 5;
        public int Fame { get; set; } = 0;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}

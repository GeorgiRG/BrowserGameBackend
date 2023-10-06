using Org.BouncyCastle.Bcpg;

namespace BrowserGameBackend.Models
{
    public class Fleet
    {
        public int Id { get; set; }
        public ICollection<ResourceStorage> ResourceStorages { get; set; } = new List<ResourceStorage>();
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}

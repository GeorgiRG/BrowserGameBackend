using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BrowserGameBackend.Models
{
	public class Planet
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public int Size { get; set; }
		public int SurfaceTemperature { get; set; } = 0;
		public int SystemPosition { get; set; }
		public int RareMetals { get; set; } = 0;
		public int Metals { get; set; } = 0;
		public int Fuels { get; set; } = 0;
		public int Organics { get; set; } = 0;
		public bool Owned { get; set; } = false;
		public int? UserId { get; set; } = null;
		public User? User { get; set; }
		public int? BotId { get; set; } = null;
		public Bot? Bot { get; set; }
        public int StarSystemId { get; set; }
		public StarSystem StarSystem { get; set; } = null!; 
		public Population? Population { get; set; }
		public ICollection<ResourceProduction>? Productions { get; set; }
    }
}

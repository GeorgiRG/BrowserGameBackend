namespace BrowserGameBackend.Models
{
	public class Planet
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public int Size { get; set; }
		public string? Type { get; set; } //Dwarf, Moon, Terrestrial, Giga Terrestrial
		public int SystemPosition { get; set; }
		public int RareMetals { get; set; }
		public int Metals { get; set; }
		public int Fuels { get; set; }
		public int Organics { get; set; }
		public int Population { get; set; } = 0;
		public int Shielding { get; set; } = 0;
		public int OwnerId { get; set; } = 0;
        public int StarSystemId { get; set; }
		public StarSystem StarSystem { get; set; } = null!;

    }
}

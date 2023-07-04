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
		public int Population { get; set; } = 0;
		public int Shielding { get; set; } = 0;
		public int OwnerId { get; set; } = 0;
        public int StarSystemId { get; set; }
		public StarSystem StarSystem { get; set; } = null!;

    }
}

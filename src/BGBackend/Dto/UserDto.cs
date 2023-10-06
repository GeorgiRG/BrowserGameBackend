namespace BrowserGameBackend.Dto

{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Faction { get; set; }
        public string Species { get; set; }
        public string SessionId { get; set; }

        public UserDto(int id, string name, string faction, string species, string sessId) 
        {
            Id = id;
            Name = name;
            Faction = faction;
            Species = species;
            SessionId = sessId;
        }
    }

}

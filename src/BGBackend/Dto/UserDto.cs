namespace BrowserGameBackend.Dto

{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Faction { get; set; }
        public string Race { get; set; }
        public string CharClass { get; set; }
        public string SessionId { get; set; }

        public UserDto(int id, string name, string email, string faction, string race, string charClass, string sessId) 
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.Faction = faction;
            this.Race = race;
            this.CharClass = charClass;
            this.SessionId = sessId;
        }
    }

}

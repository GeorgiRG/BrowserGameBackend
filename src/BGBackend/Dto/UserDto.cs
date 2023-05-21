namespace BrowserGameBackend.Dto

{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Faction { get; set; }
        public string Species { get; set; }
        public int UserSkillsId { get; set; }
        public string SessionId { get; set; }

        public UserDto(int id, string name, string faction, string species,int userSkillsId, string sessId) 
        {
            this.Id = id;
            this.Name = name;
            this.Faction = faction;
            this.Species = species;
            this.UserSkillsId = userSkillsId;
            this.SessionId = sessId;
        }
    }

}

namespace BrowserGameBackend.Models
{
    //constant/reference resource values
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public ResourceRequirements? ResourceRequirements { get; set; }
    }
}
